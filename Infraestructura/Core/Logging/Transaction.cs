namespace Infraestructura.Core.Logging
{
    public class Transaction
    {
        public Transaction()
        {
            TransactionDetail = new List<TransactionDetail>();
        }

        public Guid TransactionId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ModifiedBy { get; set; }
        public string TransactionOrigen { get; set; }
        public List<TransactionDetail> TransactionDetail { get; set; }

        public void AddDetail(string tableName, string crudOperation, string transactionType)
        {
            if (TransactionDetail.FirstOrDefault(t => t.TableName == tableName) == null)
            {
                TransactionDetail.Add(
                    new TransactionDetail
                    {
                        TransactionId = TransactionId,
                        TableName = tableName,
                        CrudOperation = crudOperation,
                        TransactionType = transactionType
                    });
            }
        }
    }
}
