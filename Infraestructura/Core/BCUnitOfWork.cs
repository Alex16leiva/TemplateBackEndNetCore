using Dominio.Core;
using Infraestructura.Context;
using Infraestructura.Core.Identity;
using Infraestructura.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Infraestructura.Core
{
    public class BCUnitOfWork : DbContext
    {
        private string Transact { get; set; }
        public BCUnitOfWork(DbContextOptions<MyContext>? context)
            : base(context)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromSeconds(1).TotalSeconds);
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public virtual void Commit(TransactionInfo? transactionInfo)
        {
            Logging.Transaction transaction = BuildTransactionInfo(transactionInfo);
            Commit(transaction, transactionInfo.GenerateTransaction);
        }

        private void Commit(Logging.Transaction transaction, bool generateTransaction)
        {
            try
            {
                base.Database.OpenConnection();
                //Reseteando el detalle de las transacciones.
                transaction.TransactionDetail = new List<Logging.TransactionDetail>();

                using (var scope = TransactionScopeFactory.GetTransactionScope())
                {
                    var changedEntities = new List<ModifiedEntityEntry>();
                    var tableMapping = new List<EntityMapping>();
                    var sqlCommandInfos = new List<SqlCommandInfo>();

                    IEnumerable<EntityEntry> changeDbEntityEntries = GetChangedDbEntityEntries();

                    foreach (EntityEntry entry in changeDbEntityEntries)
                    {
                        ApplyTransactionInfo(transaction, entry);

                        if (!generateTransaction)
                        {
                            // Get the deleted records info first
                            if (entry.State == EntityState.Deleted)
                            {
                                EntityMapping entityMapping = GetEntityMappingConfiguration(tableMapping, entry);
                                SqlCommandInfo sqlCommandInfo = GetSqlCommandInfo(transaction, entry, entityMapping);
                                if (sqlCommandInfo != null) sqlCommandInfos.Add(sqlCommandInfo);

                                transaction.AddDetail(entityMapping.TableName, entry.State.ToString(), transaction.TransactionType);
                            }
                            else
                            {
                                changedEntities.Add(new ModifiedEntityEntry(entry, entry.State.ToString()));
                            }
                        }

                    }
                    base.SaveChanges();

                    if (!generateTransaction)
                    {
                        // Get the Added and Mdified records after changes, that way we will be able to get the generated .
                        foreach (ModifiedEntityEntry entry in changedEntities)
                        {
                            EntityMapping entityMapping = GetEntityMappingConfiguration(tableMapping, entry.EntityEntry);
                            SqlCommandInfo sqlCommandInfo = GetSqlCommandInfo(transaction, entry.EntityEntry, entityMapping);
                            if (sqlCommandInfo != null) sqlCommandInfos.Add(sqlCommandInfo);
                            
                            transaction.AddDetail(entityMapping.TableName, entry.State, transaction.TransactionType);
                        }

                        // Adding Audit Detail Transaction CommandInfo.
                        sqlCommandInfos.AddRange(GetAuditRecords(transaction));

                        // Insert Transaction and audit records.
                        foreach (SqlCommandInfo sqlCommandInfo in sqlCommandInfos)
                        {
                            Database.ExecuteSqlRaw(sqlCommandInfo.Sql, sqlCommandInfo.Parameters);
                        }

                    }

                    scope.Complete();
                }
            }
            finally
            {

                base.Database.CloseConnection();
            }
        }

        private IEnumerable<SqlCommandInfo> GetAuditRecords(Logging.Transaction transaction)
        {
            var auditCommands = new List<SqlCommandInfo>();

            // Adding Audit Header Transaction CommandInfo.
            auditCommands.Add(GetAuditHeaderCommandInfo(transaction));

            // Adding Audit Detail Transaction CommandInfo
            foreach (var transactionDetail in transaction.TransactionDetail)
            {
                auditCommands.Add(GetAuditDetailCommandInfo(transactionDetail));
            }

            return auditCommands;
        }

        private SqlCommandInfo GetAuditDetailCommandInfo(TransactionDetail transactionDetail)
        {
            const string sqlInsert =
                "insert into  Comunes.LogTransaccionesDetalle(TransaccionUId,TipoTransaccion, EntidadDominio, DescripcionTransaccion) " +
                                       "values({0}, {1}, {2},{3})";

            var param = new object[]
                                 {
                                     transactionDetail.TransactionId,transactionDetail.TransactionType, transactionDetail.TableName, transactionDetail.CrudOperation
                                 };

            return new SqlCommandInfo(sqlInsert, param);
        }

        private SqlCommandInfo GetAuditHeaderCommandInfo(Logging.Transaction transaction)
        {
            const string sqlInsert =
                "insert into  Comunes.LogTransacciones(TransaccionUId, TipoTransaccion, FechaTransaccion, ModificadoPor, OrigenTransaccion) " +
                "values({0}, {1}, {2}, {3}, {4} )";

            var param = new object[]
                                 {
                                     transaction.TransactionId, transaction.TransactionType, transaction.TransactionDate,
                                     transaction.ModifiedBy, transaction.TransactionOrigen
                                 };

            return new SqlCommandInfo(sqlInsert, param);
        }

        private SqlCommandInfo GetSqlCommandInfo(Logging.Transaction transaction, EntityEntry entry, EntityMapping entityMapping)
        {
            if (entityMapping.TableName.Contains("_Transacciones"))
            {
                return null;
            }

            string sqlInsert;
            object[] param;
            CreateTransactionInsertStatement(entityMapping, entry, transaction, out sqlInsert, out param);

            var sqlCommandInfo = new SqlCommandInfo(sqlInsert, param);
            return sqlCommandInfo;
        }

        private void CreateTransactionInsertStatement(EntityMapping entityMapping, EntityEntry entry,
                                                      Logging.Transaction transaction, out string sqlInsert, out object[] objects)
        {
            var insert = new StringBuilder();
            var fields = new StringBuilder();
            var paramNames = new StringBuilder();
            var values = new List<Object>();

            insert.AppendLine(string.Format("Insert Into {0} ", entityMapping.TransactionTableName));

            int index = 0;
            IEnumerable<string> propertyNames = entry.State == EntityState.Deleted
                                                    ? GetPropertiesEntity(entry, entry.OriginalValues)
                                                    : GetPropertiesEntity(entry, entry.CurrentValues);

            foreach (string property in propertyNames)
            {
                string prop = property;
                if (prop != "RowVersion")
                {
                    if (fields.Length == 0)
                    {
                        fields.Append(string.Format(" ({0}", prop));
                        paramNames.Append(string.Format(" values ({0}{1}{2}", "{", index, "}"));
                    }
                    else
                    {
                        fields.Append(string.Format(", {0}", prop));
                        paramNames.Append(string.Format(", {0}{1}{2}", "{", index, "}"));
                    }

                    values.Add(GetEntityPropertyValue(entry, prop, transaction));
                    index++;
                }
            }

            fields.Append(string.Format(") "));
            paramNames.Append(string.Format(") "));

            insert.AppendLine(fields.ToString());
            insert.AppendLine(paramNames.ToString());

            sqlInsert = insert.ToString();
            objects = values.ToArray();
        }

        private object GetEntityPropertyValue(EntityEntry? entry, string? prop, Logging.Transaction? transaction)
        {
            object value;
            TryGeTransactionInfo(prop, transaction, out value);
            if (value != null)
            {
                return value;
            }

            if (entry.State == EntityState.Deleted || entry.State == EntityState.Detached)
            {
                return prop == "DescripcionTransaccion"
                           ? EntityState.Deleted.ToString()
                           : entry.Property(prop).OriginalValue;
            }
            return entry.Property(prop).CurrentValue;
        }

        private void TryGeTransactionInfo(string property, Logging.Transaction transaction, out object value)
        {
            switch (property)
            {
                case "TransaccionUId":
                    value = transaction.TransactionId;
                    break;

                case "TipoTransaccion":
                    value = transaction.TransactionType;
                    break;

                case "FechaTransaccion":
                    value = transaction.TransactionDate;
                    break;

                case "ModificadoPor":
                    value = transaction.ModifiedBy;
                    break;

                default:
                    value = null;
                    break;
            }
        }

        private List<string> GetPropertiesEntity(EntityEntry? entry, PropertyValues? originalValues)
        {
            List<string> propertyNames = new();
            var entity = entry.Entity;
            var entityType =  entity.GetType();

            var properties = entry.OriginalValues.Properties;

            foreach (var prop in properties)
            {
                if (entityType.GetProperty(prop.Name) == null)
                    continue;
                var pp = entityType.GetProperty(prop.Name);
                if (pp.GetValue(entity) == null)
                    continue;
                propertyNames.Add(prop.Name);
            }

            return propertyNames;
        }

        private EntityMapping GetEntityMappingConfiguration(List<EntityMapping> tableMapping, EntityEntry entry)
        {
            var type = GetDomainEntityType(entry);

            var name = entry.Metadata.GetTableName();
            var schema = entry.Metadata.GetSchema();

            var nameTable = string.Format("{0}.{1}", schema, name);

            EntityMapping entityMapping = tableMapping.FirstOrDefault(m => m.EntityType == type);
            if (entityMapping == null)
            {
                entityMapping = CreateTableMapping(type, nameTable);
                tableMapping.Add(entityMapping);
            }
            return entityMapping;
        }

        private EntityMapping CreateTableMapping(Type type, string tname)
        {
            return new EntityMapping { EntityType = type, TableName = tname, TransactionTableName = GetTransactionTableName(tname) };
        }

        private string GetTransactionTableName(string tname)
        {
            if (tname.Contains("_Transacciones"))
            {
                return tname;
            }


            string result = string.Format("{0}_Transacciones", tname);
            return result;
        }

        private Type GetDomainEntityType(EntityEntry entry)
        {
            Type type = entry.Entity.GetType();
            if (type.FullName != null)
            {
                if (type.FullName.Contains("Dominio"))
                {
                    return type;
                }
                if (type.BaseType != null)
                {
                    return type.BaseType;
                }
            }

            return null;
        }

        private void ApplyTransactionInfo(Logging.Transaction transaction, EntityEntry entry)
        {
            ((Entity)entry.Entity).FechaTransaccion = transaction.TransactionDate;
            ((Entity)entry.Entity).DescripcionTransaccion = entry.State.ToString();
            ((Entity)entry.Entity).ModificadoPor = transaction.ModifiedBy;

            AplicarInformacionTransaccion(entry, "TipoTransaccion", transaction.TransactionType);
            AplicarInformacionTransaccion(entry, "TransaccionUId", transaction.TransactionId);
        }

        private void AplicarInformacionTransaccion(EntityEntry item, string nombrePropiedad, object valorPropiedad)
        {
            if (item != null && item.Entity != null)
            {
                PropertyInfo propInfoEntity = item.Entity.GetType().GetProperty(nombrePropiedad);
                if (propInfoEntity != null)
                {
                    propInfoEntity.SetValue(item.Entity, valorPropiedad, null);
                }
            }
        }

        private IEnumerable<EntityEntry> GetChangedDbEntityEntries()
        {
            return ChangeTracker.Entries().Where(
                e =>
                (e.Entity is Entity) &&
                (e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted));
        }

        private Logging.Transaction BuildTransactionInfo(TransactionInfo transactionInfo)
        {
            var transaccionId = NewSequentialTransactionIdentity();

            return new Logging.Transaction
            {
                TransactionId = transaccionId.TransactionId,
                TransactionDate = transaccionId.TransactionDate,
                TransactionOrigen = transactionInfo.TipoTransaccion,
                TransactionType = transactionInfo.TipoTransaccion,
                ModifiedBy = transactionInfo.ModificadoPor
            };
        }

        public TransactionIdentity NewSequentialTransactionIdentity()
        {
            return new TransactionIdentity
            {
                TransactionId = NewSequentialGuid(),
                TransactionDate = DateTime.Now,
                TransactionUtcDate = DateTime.UtcNow
            };
        }

        public static Guid NewSequentialGuid()
        {
            byte[] uid = Guid.NewGuid().ToByteArray();
            byte[] binDate = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

            var secuentialGuid = new byte[uid.Length];

            secuentialGuid[0] = uid[0];
            secuentialGuid[1] = uid[1];
            secuentialGuid[2] = uid[2];
            secuentialGuid[3] = uid[3];
            secuentialGuid[4] = uid[4];
            secuentialGuid[5] = uid[5];
            secuentialGuid[6] = uid[6];
            // set the first part of the 8th byte to '1100' so
            // later we'll be able to validate it was generated by us

            secuentialGuid[7] = (byte)(0xc0 | (0xf & uid[7]));

            // the last 8 bytes are sequential,
            // it minimizes index fragmentation
            // to a degree as long as there are not a large
            // number of Secuential-Guids generated per millisecond

            secuentialGuid[9] = binDate[0];
            secuentialGuid[8] = binDate[1];
            secuentialGuid[15] = binDate[2];
            secuentialGuid[14] = binDate[3];
            secuentialGuid[13] = binDate[4];
            secuentialGuid[12] = binDate[5];
            secuentialGuid[11] = binDate[6];
            secuentialGuid[10] = binDate[7];

            return new Guid(secuentialGuid);
        }

        public void RollbackChanges()
        {
            //Set all entities in change tracker
            //as 'unchanged state'
            ChangeTracker.Entries()
                .ToList().ForEach(e => e.State = EntityState.Unchanged);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Database.ExecuteSqlRaw(sqlCommand, parameters);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlCommand, params object[] parameters) 
        {
            //return Set<TEntity>().FromSqlRaw(sqlCommand, parameters).ToList();

            return Database.SqlQueryRaw<TEntity>(sqlCommand, parameters);
        }

        public TType ExecuteScalarFunction<TType>(string scalarFunction, params object[] parameters)
        {
            var returnValue = Database.SqlQueryRaw<TType>(scalarFunction, parameters);

            return returnValue.FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(string sqlCommand, params object[] parameters) where TEntity: class
        {
            return await Set<TEntity>().FromSqlRaw(sqlCommand, parameters).ToListAsync();
        }

        public DbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            //Attach and set as unchanged
            Entry(item).State = EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            //This operation also attach item in object state manager
            Entry(item).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class
        {
            Entry(original).CurrentValues.SetValues(current);
        }
    }
}
