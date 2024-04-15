using Dominio.Core;
using Infraestructura.Core;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace Infraestructura.Context
{
    public interface IGenericRepository<T> : IDisposable
        where T : IQueryableUnitOfWork
    {
        /// <summary>
        /// Get the unit of work in this repository.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add the entity to the repository.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">the new entity to add.</param>
        void Add<TEntity>(TEntity entity)
            where TEntity : Entity;

        /// <summary>
        /// Add the entity to the repository.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="entity">The new entity to add</param>
        /// <returns></returns>
        Task AddAsync<TEntity>(TEntity entity)
            where TEntity : Entity;

        /// <summary>
        /// Add the entities to the repository
        /// </summary>
        /// <typeparam name="TEntity">the entity type</typeparam>
        /// <param name="entities">The new entities to add</param>
        void AddRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity;

        /// <summary>
        /// Add the entities to the repository.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="entities">The new entities to add</param>
        /// <returns></returns>
        Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity;

        /// <summary>
        /// Remove the specified entity
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="entity">The entity to remove</param>
        void Remove<TEntity>(TEntity entity) 
            where TEntity : Entity;

        /// <summary>
        /// Remove the specified from the repository
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="entities">The entities to remove</param>
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity;

        /// <summary>
        /// Get All rows.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <returns>{List{`0}}</returns>
        IEnumerable<TEntity> GetAll<TEntity>() 
            where TEntity : Entity;

        /// <summary>
        /// Get all row asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <returns>Task{List{`0}}</returns>
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() 
            where TEntity : Entity;

        /// <summary>
        /// Get All rows.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="includes">Related entities to include in the result set</param>
        /// <returns>List{`0}</returns>
        IEnumerable<TEntity> GetAll<TEntity>(List<string> includes) 
            where TEntity : Entity;

        /// <summary>
        /// Get All rows asynchronously
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="includes">Related entities to include in the result set</param>
        /// <returns>Task{List{`0}}</returns>
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(List<string> includes)
            where TEntity : Entity;

        /// <summary>
        /// Get the first or default row filtered by the query expression.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="predicate">The where (the qyery expression)</param>
        /// <returns>Object of the TEntity class</returns>
        TEntity GetSingle<TEntity>(Expression<Func<TEntity, bool>> predicate) 
            where TEntity : Entity;

        /// <summary>
        /// Get the first or default row filtered by the query expression.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="predicate">The where (the query expression)</param>
        /// <returns>Object of the TEntity class</returns>
        Task<TEntity> GetSingleAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity;

        /// <summary>
        /// Get an element of type TEntity in repository
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="predicate">Filter that the element do match</param>
        /// <param name="includes">Related entities to include in the result set</param>
        /// <returns>Selected element</returns>
        TEntity GetSingle<TEntity>(Expression<Func<TEntity, bool>> predicate, List<string> includes)
            where TEntity : Entity;

        /// <summary>
        /// Get element of type TEntity in repository
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="predicate">Filter that the element do match</param>
        /// <param name="includes">Related entities to include in the result set</param>
        /// <returns>Selected element</returns>
        Task<TEntity> GetSingleAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, List<string> includes)
            where TEntity : Entity;

        /// <summary>
        /// Gets filtered entities
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="predicate">The where.</param>
        /// <returns>Enumerable of the TEntity class</returns>
        IEnumerable<TEntity> GetFiltered<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity;

        /// <summary>
        /// Gets the many asynchronous
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="predicate">The where</param>
        /// <returns>Enumerable of the TEntity class</returns>
        Task<IEnumerable<TEntity>> GetFilteredAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity: Entity;

        /// <summary>
        /// Gets the many entities
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="predicate">The where</param>
        /// <returns>Enumerable of the TEntity class</returns>
        IEnumerable<TEntity> GetFiltered<TEntity>(Expression<Func<TEntity, bool>> predicate, List<string> includes)
            where TEntity : Entity;

        /// <summary>
        /// Gets the many asynchronous
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="predicate">The where</param>
        /// <returns>Enumerable of the TEntity class</returns>
        Task<IEnumerable<TEntity>> GetFilteredAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, List<string> includes)
            where TEntity : Entity;

        PagedCollection GetPagedAndFiltered<TEntity>(DynamicFilter filterDef)
            where TEntity : Entity;

        Task<PagedCollection> GetPagedAndFilteredAsync<TEntity>(DynamicFilter filterDef)
            where TEntity : Entity;

        /// <summary>
        /// Gets Modify entities
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <param name="item">The where</param>
        void Modify<TEntity>(TEntity item)
            where TEntity : Entity;

        /// <summary>
        /// Execute specific stored procedure with underliying persistence store
        /// </summary>
        /// <typeparam name="TType">Entity type to map query results</typeparam>
        /// <param name="storedProcedure">
        /// The Stored Procedure name 
        /// <example>
        /// ImportacionExportacion.spControlInvoiceLineaProductoCP
        /// </example>
        /// </param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>
        /// Enumerable results 
        /// </returns>
        IEnumerable<TType> ExecuteStoredProcedure<TType>(string storedProcedure, Dictionary<string, object> parameters);
        IEnumerable<TType> ExecuteStoredProcedure<TType>(string storedProcedure, SqlParameter[] parameters);

        /// <summary>
        /// Execute specific scalar function with underliying persistence store
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="scalarFunction"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        TType ExecuteScalarFunction<TType>(string scalarFunction, Dictionary<string, object> parameters);

        void ExecuteQuery(string sqlQuery, Dictionary<string, object> parameters);

        void ExecuteQuery(SqlParameter[] parms, string sqlQuery);

        /// <summary>
        /// Validate is running any jobs
        /// </summary>
        /// <param name="jobNames">Job names to be valid</param>
        /// <returns>If a job is run from the jobNames list, it returns true</returns>
        Task<bool> IsRunningJobsAsync(string jobName);

        IEnumerable<TEntity> ExecuteQuery<TEntity>(SqlParameter[] parms, string sqlQuery);
    }
}
