using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.Domain.Exceptions;
using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;
using NpgSQL.CRUDBuilder.Domain.Providers.Models;

namespace NpgSQL.CRUDBuilder.Domain.Providers
{
    public class DataOverTransactionProvider
    {
        public async Task ExecuteNonQuery(NpgsqlConnection connection,
            Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> dataPreValidateDelegate,
            Func<ITransactionArgumentsModel, bool> dataPostValidateDelegate = null,
            CancellationToken cancellationToken = default)
        {
            var transactionExecuteState = await RunDataTransaction(connection, buildQueryDelegate,
                transactionArgumentsModel, dataPreValidateDelegate, false, 
                dataPostValidateDelegate, cancellationToken);

            if (transactionExecuteState.State is TransactionResultState.Failed)
            {
                
            }
        }

        public async Task<TData> ExecuteData<TData>(NpgsqlConnection connection,
            Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> dataPreValidateDelegate,
            Func<ITransactionArgumentsModel, bool> dataPostValidateDelegate = null,
            CancellationToken cancellationToken = default) where TData : class
        {
            throw new NotImplementedException();
        }

        private async Task<TransactionResult> RunDataTransaction(NpgsqlConnection connection,
            Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> preValidateDelegate,
            bool isDataRequestTransaction,
            Func<ITransactionArgumentsModel, bool> postValidateDelegate = null,
            CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new TransactionResult(TransactionResultState.Canceled);
            }

            if (!preValidateDelegate(transactionArgumentsModel))
            {
                throw new InvalidTransactionArgumentException();
            }

            if (!(postValidateDelegate is null))
            {
                if (!postValidateDelegate(transactionArgumentsModel))
                {
                    throw new InvalidTransactionArgumentException();
                }
            }

            if (!(connection.State is ConnectionState.Open
                  || connection.State is ConnectionState.Closed))
            {
                throw new CorruptedConnectionException();
            }

            return await new TransactionsProvider().ExecuteTransaction(connection, buildQueryDelegate,
                transactionArgumentsModel, isDataRequestTransaction);
        }
    }
}