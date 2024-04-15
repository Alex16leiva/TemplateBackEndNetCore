
using Dominio.Core;
using Dominio.Core.Extensions;
using Infraestructura.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Dynamic;
using System.Linq.Expressions;


namespace Infraestructura.Context
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : IQueryableUnitOfWork
    {
        private readonly T _unitOfWork;
        private readonly IConfiguration _configuration;
        public GenericRepository(T unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        private DbSet<TEntity> GetSet<TEntity>() where TEntity : class
        {
            return _unitOfWork.CreateSet<TEntity>();
        }

        public IUnitOfWork UnitOfWork
        { 
            get { return _unitOfWork; } 
        }

        /// <inheritdoc/>
        public void Add<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (entity.IsNotNull())
            {
                entity.FechaTransaccion = DateTime.Now;
                entity.DescripcionTransaccion = "Insert";
                entity.RowVersion = Array.Empty<Byte>();
                GetSet<TEntity>().Add(entity); //Add new item in this set
            }
        }

        /// <inheritdoc/>
        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (entity.IsNotNull())
            {
                entity.FechaTransaccion = DateTime.Now;
                entity.DescripcionTransaccion = "Insert";
                entity.RowVersion = Array.Empty<Byte>();
                await GetSet<TEntity>().AddAsync(entity); //Add new item in this set
            }
        }

        /// <inheritdoc/>
        public void AddRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            if (entities.HasItems())
            {
                GetSet<TEntity>().AddRange(entities);
            }
        }

        /// <inheritdoc/>
        public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) 
            where TEntity : Entity
        {
            if (entities.HasItems())
            {
                await GetSet<TEntity>().AddRangeAsync(entities);
            }
        }

        public void Dispose()
        {
            if (_unitOfWork.IsNotNull())
            {
                _unitOfWork.Dispose();
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll<TEntity>() 
            where TEntity : Entity
        {
            return GetSet<TEntity>().ToList();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : Entity
        {
            return await GetSet<TEntity>().ToListAsync();
        }
        
        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll<TEntity>(List<string> includes) 
            where TEntity : Entity
        {
            IQueryable<TEntity> items = GetSet<TEntity>();

            if (includes.HasItems())
            {
                //Adding Includes to filter.
                items = includes.Aggregate(items, (current, include) => current.Include(include));
            }

            return items.ToList();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(List<string> includes)
            where TEntity : Entity
        {
            IQueryable<TEntity> items = GetSet<TEntity>();

            if (includes.HasItems())
            {
                //Adding Includes to filter.
                items = includes.Aggregate(items, (current, include) => current.Include(include));
            }

            return await items.ToListAsync();
        }

        /// <inheritdoc/>
        public TEntity GetSingle<TEntity>(Expression<Func<TEntity, bool>> predicate) 
            where TEntity : Entity
        {
            return GetSet<TEntity>().FirstOrDefault(predicate);
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetSingleAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) 
            where TEntity : Entity
        {
            return await GetSet<TEntity>().FirstOrDefaultAsync(predicate);
        }


        /// <inheritdoc/>
        public TEntity GetSingle<TEntity>(Expression<Func<TEntity, bool>> predicate, List<string> includes)
            where TEntity : Entity
        {
            IQueryable<TEntity> items = GetSet<TEntity>();

            if (includes.HasItems())
            {
                //Adding include to the filter.
                items = includes.Aggregate(items, (current, include) => current.Include(include));
            }

            return items.FirstOrDefault(predicate);
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetSingleAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, List<string> includes)
            where TEntity : Entity
        {
            IQueryable<TEntity> items = GetSet<TEntity>();

            if (includes.HasItems())
            {
                //Adding include to the filter.
                items = includes.Aggregate(items, (current, include) => current.Include(include));
            }

            return await items.FirstOrDefaultAsync(predicate);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetFiltered<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity
        {
            return GetSet<TEntity>().Where(predicate).ToList();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetFilteredAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity
        {
            return await GetSet<TEntity>().Where(predicate).ToListAsync();
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetFiltered<TEntity>(Expression<Func<TEntity, bool>> predicate, List<string> includes)
            where TEntity : Entity
        {
            IQueryable<TEntity> items = GetSet<TEntity>();
            if (includes.HasItems())
            {
                //Adding includes to filter
                items = includes.Aggregate(items, (current, include) => current.Include(include));
            }

            return items.Where(predicate).ToList();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetFilteredAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, List<string> includes)
            where TEntity : Entity
        {
            IQueryable<TEntity> items = GetSet<TEntity>();
            if (includes.HasItems())
            {
                //Adding includes to filter
                items = includes.Aggregate(items, (current, include) => current.Include(include));
            }

            return await items.Where(predicate).ToListAsync();
        }

        public PagedCollection GetPagedAndFiltered<TEntity>(DynamicFilter filterDef)
            where TEntity : Entity
        {
            IQueryable<TEntity> items = !string.IsNullOrWhiteSpace(filterDef.Filtro)
                                            ? GetSet<TEntity>().Where(filterDef.Filtro, filterDef.Valores)
                                            : GetSet<TEntity>();

            if (filterDef.Includes.HasItems())
            {
                //Adding Includes to the filter
                items = filterDef.Includes.Aggregate(items, (current, include) => current.Include(include));
            }

            int totalItems = items.Count();

            if (filterDef.PageSize != 0)
            {
                //Adding sort criteria.
                if (filterDef.SortFields.HasItems())
                {
                    string orderKey = filterDef.Ascending ? "ASC" : "DESC";

                    var order = string.Join(" " + orderKey + ", ", filterDef.SortFields.ToArray());

                    if (!order.EndsWith(orderKey))
                    {
                        order += " " + orderKey;
                    }

                    items = items.OrderBy(order);

                    items = items.Skip(filterDef.PageSize * filterDef.PageIndex);
                }

                items = items.Take(filterDef.PageSize);
            }

            var pagedItems = items.ToList();

            return new PagedCollection(filterDef.PageIndex, filterDef.PageSize, pagedItems, totalItems, pagedItems.Count());
        }

        public async Task<PagedCollection> GetPagedAndFilteredAsync<TEntity>(DynamicFilter filterDef)
            where TEntity : Entity
        {
            IQueryable<TEntity> items = !string.IsNullOrWhiteSpace(filterDef.Filtro)
                                            ? GetSet<TEntity>().Where(filterDef.Filtro, filterDef.Valores)
                                            : GetSet<TEntity>();

            if (filterDef.Includes.HasItems())
            {
                //Adding Includes to the filter
                items = filterDef.Includes.Aggregate(items, (current, include) => current.Include(include));
            }

            int totalItems = items.Count();

            if (filterDef.PageSize != 0)
            {
                //Adding sort criteria.
                if (filterDef.SortFields.HasItems())
                {
                    string orderKey = filterDef.Ascending ? "ASC" : "DESC";

                    var order = string.Join(" " + orderKey + ", ", filterDef.SortFields.ToArray());

                    if (!order.EndsWith(orderKey))
                    {
                        order += " " + orderKey;
                    }

                    items = items.OrderBy(order);

                    items = items.Skip(filterDef.PageSize * filterDef.PageIndex);
                }

                items = items.Take(filterDef.PageSize);
            }

            var pagedItems = await items.ToListAsync();

            return new PagedCollection(filterDef.PageIndex, filterDef.PageSize, pagedItems, totalItems, pagedItems.Count());
        }

        /// <inheritdoc/>
        public void Remove<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            if (entity.IsNotNull())
            {
                //Attach item if not exist
                _unitOfWork.Attach(entity);

                //set as "Remove"
                GetSet<TEntity>().Remove(entity);
            }
        }

        /// <inheritdoc/>
        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) 
            where TEntity : Entity
        {
            if (entities.HasItems())
            {
                //set as removed
                GetSet<TEntity>().RemoveRange(entities);
            }
        }

        /// <inheritdoc/>
        public void Modify<TEntity>(TEntity item)
            where TEntity : Entity
        {
            if (item.IsNotNull())
            {
                _unitOfWork.SetModified(item);
            }
        }

        public IEnumerable<TType> ExecuteStoredProcedure<TType>(string storedProcedure, Dictionary<string, object> parameters)
        {
            SqlParameter[] sqlParameters = CreateSqlParameters(parameters);
            string paramNames = GetParamNames(parameters);

            return (string.IsNullOrWhiteSpace(paramNames))
                ? _unitOfWork.ExecuteQuery<TType>(string.Format("EXEC {0}", storedProcedure), sqlParameters).ToList()
                : _unitOfWork.ExecuteQuery<TType>(string.Format("EXEC {0} {1}", storedProcedure, paramNames), sqlParameters).ToList();
        }

        public IEnumerable<TType> ExecuteStoredProcedure<TType>(string storedProcedure, SqlParameter[] parameters)
        {
            string paramNames = GetParamNames(parameters);
            return _unitOfWork.ExecuteQuery<TType>(string.Format("EXEC {0} {1}", storedProcedure, paramNames), parameters).ToList();
        }

        public TType ExecuteScalarFunction<TType>(string scalarFunction, Dictionary<string, object> parameters)
        {
            SqlParameter[] sqlParameters = CreateSqlParameters(parameters);
            string paramNames = GetParamNames(parameters);

            var result = (string.IsNullOrWhiteSpace(paramNames))
                ? _unitOfWork.ExecuteScalarFunction<TType>(string.Format("SELECT {0}();", scalarFunction), sqlParameters)
                : _unitOfWork.ExecuteScalarFunction<TType>(string.Format("SELECT {0}({1});", scalarFunction, paramNames), sqlParameters);

            return result;
        }

        private string GetParamNames(Dictionary<string, object> parameters)
        {
            return (parameters != null && parameters.Any())
                ? parameters.Select(p => p.Key).Aggregate((i, j) => i + ", " + j)
                : string.Empty;
        }

        private string GetParamNames(SqlParameter[] parameters)
        {
            return (parameters != null && parameters.Any())
                ? parameters.Select(p => p.ParameterName).Aggregate((i, j) => i + ", " + j)
                : string.Empty;
        }

        public void ExecuteQuery(string sqlQuery, Dictionary<string, object> parameters)
        {
            SqlParameter[] sqlParameters = CreateSqlParameters(parameters);
            _unitOfWork.ExecuteCommand(sqlQuery, sqlParameters);
        }

        private SqlParameter[] CreateSqlParameters(Dictionary<string, object> parameters)
        {
            if (parameters != null && parameters.Any())
            {
                return (from qry in parameters select new SqlParameter(qry.Key, qry.Value)).ToArray();
            }

            return new SqlParameter[0];
        }

        public void ExecuteQuery(SqlParameter[] parms, string sqlQuery)
        {
            _unitOfWork.ExecuteCommand(sqlQuery, parms);
        }

        public async Task<bool> IsRunningJobsAsync(string jobName)
        {
            string connectionString = _configuration.GetConnectionString("conectionDataBase");
            bool result = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta para verificar si el trabajo está siendo ejecutado actualmente
                    string query = $"SELECT COUNT(*) FROM msdb.dbo.sysjobs j " +
                        $"INNER JOIN msdb.dbo.sysjobactivity a " +
                        $"  ON j.job_id = a.job_id " +
                        $"WHERE j.name = '{jobName}' AND a.run_requested_date IS NOT NULL AND a.stop_execution_date IS NULL";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int runningJobCount = (int)command.ExecuteScalar();

                        if (runningJobCount > 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: { ex.Message }");
            }

            return result;
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(SqlParameter[] parms, string sqlQuery)
        {
            return _unitOfWork.ExecuteQuery<TEntity>(sqlQuery, parms).ToList();
        }
    }
}
