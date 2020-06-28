using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;

namespace NpgSQL.CRUDBuilder.Domain.Providers
{
    public class TransactionsProvider
    {
        private readonly NpgsqlConnection _npgsqlConnection;

        public TransactionsProvider(NpgsqlConnection npgsqlConnection)
        {
            _npgsqlConnection = npgsqlConnection;
        }

        public async Task<TData> ExecuteWithData<TData>(Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> preValidateFunction,
            Func<ITransactionArgumentsModel, bool> postValidateFunction = null,
            Exception preValidateException = null, Exception postValidateException = null,
            bool tryRestoreTransactionState = true, CancellationToken cancellationToken = default) where TData : class
        {
            throw new NotImplementedException();
        } 

        public async Task ExecuteNonQueryAsync(Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> preValidateFunction,
            Func<ITransactionArgumentsModel, bool> postValidateFunction = null,
            Exception preValidateException = null, Exception postValidateException = null,
            bool tryRestoreTransactionState = true, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                if (_npgsqlConnection.State == ConnectionState.Open)
                {
                    await _npgsqlConnection.DisposeAsync();
                }

                return;
            }

            if (!preValidateFunction(transactionArgumentsModel))
            {
                throw preValidateException ?? new ArgumentException();
            }

            if (postValidateFunction != null)
            {
                if (!postValidateFunction(transactionArgumentsModel))
                {
                    throw postValidateException ?? new ArgumentException();
                }
            }

            if (_npgsqlConnection.State == ConnectionState.Closed)
            {
                await TryToExecuteTransactionDelegate(_npgsqlConnection.OpenAsync, tryRestoreTransactionState);
            }

            var query = buildQueryDelegate(transactionArgumentsModel);

            await using var sqlCommand = new NpgsqlCommand(query)
            {
                Connection = _npgsqlConnection
            };

            await TryToExecuteTransactionDelegate(sqlCommand.ExecuteNonQueryAsync, tryRestoreTransactionState);
        }

        private async Task TryToExecuteTransactionDelegate(Func<Task> transactionDelegate,
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

                    await transactionDelegate();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}