using Dominio.Core;

namespace Infraestructura.Core
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        ///If the entity have fixed properties and any optimistic concurrency problem exist,
        ///then and exception is thrown.
        ///</remarks> 
        void Commit();

        /// <summary>
        /// Commit all changes made a container.
        /// </summary>
        /// <remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exist,
        /// then an exception is thrown.
        /// Also log information for the transaction
        /// </remarks>
        /// <param name="transactionInfo">Client's information to add to the transaction's info</param>
        void Commit(TransactionInfo transactionInfo);
    }
}
