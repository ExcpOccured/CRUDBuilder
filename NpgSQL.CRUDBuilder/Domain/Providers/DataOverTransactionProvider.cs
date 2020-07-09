using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.Domain.Exceptions;
using NpgSQL.CRUDBuilder.Domain.Mapping;
using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;
using NpgSQL.CRUDBuilder.Domain.Providers.Models;
using NpgSQL.CRUDBuilder.Domain.Resolvers;

namespace NpgSQL.CRUDBuilder.Domain.Providers
{
    public class DataOverTransactionProvider
    {
        public async Task ExecuteNonQuery(NpgsqlConnection connection,
            Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> argsValidateDelegate,
            CancellationToken cancellationToken = default)
        {
            var transactionExecuteState = await RunDataTransaction(connection, buildQueryDelegate,
                transactionArgumentsModel, argsValidateDelegate, false,
                cancellationToken);

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (transactionExecuteState.State is TransactionResultState.Canceled)
            {
                return;
            }

            if (transactionExecuteState.State is TransactionResultState.Failed)
            {
                TransactionExceptionResolver.TryResolve(transactionExecuteState);
            }
        }

        public async Task<TData> ExecuteData<TData>(NpgsqlConnection connection,
            Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> argsValidateDelegate,
            CancellationToken cancellationToken = default) where TData : class
        {
            var transactionExecuteState = await RunDataTransaction(connection, buildQueryDelegate,
                transactionArgumentsModel, argsValidateDelegate, true,
                cancellationToken);

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (transactionExecuteState.State is TransactionResultState.Canceled)
            {
                return default;
            }

            if (transactionExecuteState.State is TransactionResultState.Failed)
            {
                TransactionExceptionResolver.TryResolve(transactionExecuteState);
            }

            return DtoPropsMapper.TryMapDtoProps<TData>(transactionExecuteState.DataReader);
        }

        private async Task<TransactionResult> RunDataTransaction(NpgsqlConnection connection,
            Func<ITransactionArgumentsModel, string> buildQueryDelegate,
            ITransactionArgumentsModel transactionArgumentsModel,
            Func<ITransactionArgumentsModel, bool> argsValidateDelegate,
            bool isDataRequestTransaction,
            CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new TransactionResult(TransactionResultState.Canceled);
            }

            if (argsValidateDelegate(transactionArgumentsModel) == false)
            {
                throw new InvalidTransactionArgumentException();
            }

            if ((connection.State == ConnectionState.Open
                 || connection.State == ConnectionState.Closed) == false)
            {
                throw new CorruptedConnectionException();
            }

            return await new TransactionsProvider().ExecuteTransaction(connection, buildQueryDelegate,
                transactionArgumentsModel, isDataRequestTransaction);
        }
    }
}