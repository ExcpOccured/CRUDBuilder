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
                await TryToExecuteTransactionDelegate(connection.OpenAsync, tryRestoreTransactionState);
            }

            var query = buildQueryDelegate(transactionArgumentsModel);

            await using var sqlCommand = new NpgsqlCommand(query)
            {
                Connection = connection
            };

            await TryToExecuteTransactionDelegate(sqlCommand.ExecuteNonQueryAsync, tryRestoreTransactionState);
        }

        private async Task<TransactionResult> TryToExecuteTransactionDelegate(
            Func<Task> transactionDelegate,
            bool tryRestoreTransactionState = true)
        {
            try
            {
                await transactionDelegate();
            }
            catch (PostgresException exception)
            {
                if (tryRestoreTransactionState && exception.IsTransient)
                {
                    await Task.Delay(500);

                    try
                    {
                        await transactionDelegate();
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

            return new TransactionResult(TransactionResultState.Completed);
        }

        private async Task<TransactionResult> TryToExecuteTransactionDelegate(
            Func<Task<NpgsqlDataReader>> transactionDelegate,
            bool tryRestoreTransactionState = true)
        {
            NpgsqlDataReader reader;

            try
            {
                reader = await transactionDelegate();
            }
            catch (PostgresException exception)
            {
                if (tryRestoreTransactionState && exception.IsTransient)
                {
                    await Task.Delay(500);

                    try
                    {
                        await transactionDelegate();
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
            
            
        }
    }
}