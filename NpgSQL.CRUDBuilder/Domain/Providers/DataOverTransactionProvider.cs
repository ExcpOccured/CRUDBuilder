using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;

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
            var transactionProvider = 
        }

        public async Task<TData> ExecuteData<TData>(NpgsqlConnection connection,
            Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> dataPreValidateDelegate,
            Func<ITransactionArgumentsModel, bool> dataPostValidateDelegate = null,
            CancellationToken cancellationToken = default) where TData : class
        {
        }

        private async Task RunDataTransaction(NpgsqlConnection connection,
            Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> dataPreValidateDelegate,
            Func<ITransactionArgumentsModel, bool> dataPostValidateDelegate = null,
            CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.DisposeAsync();
                }

                return;
            }

            if (!dataPreValidateDelegate(transactionArgumentsModel))
            {
                // TODO: Create exception type resolver
                throw new ArgumentException();
            }

            if (dataPostValidateDelegate != null)
            {
                if (!dataPostValidateDelegate(transactionArgumentsModel))
                {
                    throw new ArgumentException();
                }
            }

            var transactionProvider = new TransactionsProvider(connection);

            var transactionResult = await transactionProvider.ExecuteTransaction(buildQueryDelegate,)
        }
    }
}