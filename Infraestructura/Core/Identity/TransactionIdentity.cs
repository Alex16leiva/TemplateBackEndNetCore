namespace Infraestructura.Core.Identity
{
    public class TransactionIdentity
    {
        /// <summary>
        /// Identity's transaction.
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Server's Date and Time
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// UTC date and time for the transaction.
        /// </summary>
        public DateTime TransactionUtcDate { get; set; }
    }
}
