using System;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.Interfaces;
using NpgSQL.CRUDBuilder.SDK.Commands.Builders;

namespace NpgSQL.CRUDBuilder
{
    public class NpgSqlClient : INpgSqlClient
    {
        public async Task ExecuteNonQuery<TArguments>(NpgsqlConnection connection,
            Func<TArguments, string> buildQueryDelegate, TArguments delegateArgsModel,
            bool keepConnectionOpen = true, CancellationToken cancellationToken = default) =>
            await new QueryDelegateCommandBuilder().ExecuteNonQuery(connection, buildQueryDelegate(delegateArgsModel),
                keepConnectionOpen, cancellationToken);

        public async Task<TModel> ExecuteData<TModel, TArguments>(NpgsqlConnection connection,
            Func<TArguments, string> buildQueryDelegate, TArguments delegateArgsModel,
            bool keepConnectionOpen = true, CancellationToken cancellationToken = default) where TModel : class =>
            await new QueryDelegateCommandBuilder().ExecuteData<TModel>(connection,
                buildQueryDelegate(delegateArgsModel),
                keepConnectionOpen, cancellationToken);
    }
}