using System;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;
using NpgSQL.CRUDBuilder.Domain.Providers.Models;

namespace NpgSQL.CRUDBuilder.Domain.Providers
{
    public class TransactionsProvider
    {
        public async Task<TransactionResult> ExecuteTransaction(NpgsqlConnection connection,
            Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel, bool isDataRequestTransaction,
            bool tryRestoreTransactionState = true)
        {
            if (connection.State == ConnectionState.Closed)
            {
                var connectionOpenState =
                    await TryToExecuteTransactionDelegate(new TransactionDelegate(connection.OpenAsync),
                        tryRestoreTransactionState);

                if (connectionOpenState.State is TransactionResultState.Failed)
                {
                    return connectionOpenState;
                }
            }

            var query = buildQueryDelegate(transactionArgumentsModel);

            await using var sqlCommand = new NpgsqlCommand(query)
            {
                Connection = connection
            };

            TransactionResult executeDataTransactionState;

            if (isDataRequestTransaction)
            {
                executeDataTransactionState =
                    await TryToExecuteTransactionDelegate(new TransactionDelegate(null!,
                        sqlCommand.ExecuteReaderAsync), tryRestoreTransactionState);
            }
            else
            {
                executeDataTransactionState = await TryToExecuteTransactionDelegate(
                    new TransactionDelegate(sqlCommand.ExecuteNonQueryAsync), tryRestoreTransactionState);
            }

            return executeDataTransactionState;
        }

        private async Task<TransactionResult> TryToExecuteTransactionDelegate(
            TransactionDelegate transactionDelegate,
            bool tryRestoreTransactionState = true)
        {
            NpgsqlDataReader reader = null;

            try
            {
                if (transactionDelegate.QueryWithDataDelegate is null)
                {
                    await transactionDelegate.NonQueryDelegate();
                }
                else
                {
                    reader = await transactionDelegate.QueryWithDataDelegate();
                }
            }
            catch (PostgresException exception)
            {
                if (tryRestoreTransactionState && exception.IsTransient)
                {
                    await Task.Delay(500);

                    try
                    {
                        if (transactionDelegate.QueryWithDataDelegate is null)
                        {
                            await transactionDelegate.NonQueryDelegate();
                        }
                        else
                        {
                            reader = await transactionDelegate.QueryWithDataDelegate();
                        }
                    }
                    catch (PostgresException reThrowException)
                    {
                        return new TransactionResult(TransactionResultState.Failed, reThrowException);
                    }
                }
                else
                {
                    return new TransactionResult(TransactionResultState.Failed, exception);
                }
            }

            if (transactionDelegate.QueryWithDataDelegate is null)
            {
                return new TransactionResult(TransactionResultState.Completed);
            }

            return reader is null 
                ? new TransactionResult(TransactionResultState.Failed) 
                : new TransactionResult(TransactionResultState.Completed, npgsqlDataReader: reader);
        }
    }
}