using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.SDK.Exceptions;
using NpgSQL.CRUDBuilder.SDK.Mapping;
using NpgSQL.CRUDBuilder.SDK.Providers.Models;

[assembly: InternalsVisibleTo("NpgSQL.CRUDBuilder")]

namespace NpgSQL.CRUDBuilder.SDK.Commands
{
    internal class NpgSqlCrudCommand
    {
        internal async Task ExecuteNonQuery(NpgsqlConnection connection,
            string query, CancellationToken cancellationToken = default)
        {
            var transactionExecuteState = await ExecuteTransaction(connection, query,
                false, cancellationToken);

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (transactionExecuteState.State is TransactionResultState.Canceled)
            {
                return;
            }

            if (transactionExecuteState.State is TransactionResultState.Failed)
            {
                TransactionExceptionMapper.TryToMap(transactionExecuteState);
            }
        }

        internal async Task<TData> ExecuteData<TData>(NpgsqlConnection connection,
            string query, CancellationToken cancellationToken = default) where TData : class
        {
            var transactionExecuteState = await ExecuteTransaction(connection, query, true,
                cancellationToken);

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (transactionExecuteState.State is TransactionResultState.Canceled)
            {
                return default;
            }

            if (transactionExecuteState.State is TransactionResultState.Failed)
            {
                TransactionExceptionMapper.TryToMap(transactionExecuteState);
            }

            return DtoPropsMapper.TryMapDtoProps<TData>(transactionExecuteState.DataReader);
        }

        private async Task<TransactionResult> ExecuteTransaction(NpgsqlConnection connection,
            string query, bool isDataRequestTransaction,
            CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new TransactionResult(TransactionResultState.Canceled);
            }

            if ((connection.State == ConnectionState.Open
                 || connection.State == ConnectionState.Closed) == false)
            {
                throw new CorruptedConnectionException();
            }

            return await RunTransaction(connection, query, isDataRequestTransaction);
        }

        private async Task<TransactionResult> RunTransaction(NpgsqlConnection connection, string query,
            bool isDataRequestTransaction,
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
                    await Task.Delay(TimeSpan.FromSeconds(5));

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

            if (reader is null)
            {
                return new TransactionResult(TransactionResultState.Failed,
                    new InnerException($"An exception occurred during initialization of {nameof(NpgsqlDataReader)}"));
            }

            return new TransactionResult(TransactionResultState.Completed, npgsqlDataReader: reader);
        }
    }
}