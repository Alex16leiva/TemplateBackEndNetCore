using Aplicacion.DTOs;
using Dominio.Core;

namespace Aplicacion.Helpers
{
    public static class TransactionInfoExtensions
    {
        public static TransactionInfo CrearTransactionInfo(this RequestUserInfo requestUserInfo, string transactionId)
        {
            return TransactionInfoHelper.CrearTransactionInfo(requestUserInfo, transactionId);
        }

        public static TransactionInfo CrearTransactionInfo(this RequestUserInfo requestUserInfo)
        {
            return TransactionInfoHelper.CrearTransactionInfo(requestUserInfo);
        }
    }
}
