using System;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace NpgSQL.CRUDBuilder.Interfaces
{
    public interface INpgSqlClient
    {
        Task ExecuteNonQuery<TArguments>(NpgsqlConnection connection,
            Func<TArguments, string> buildQueryDelegate,
            TArguments delegateArgsModel,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default);

        Task<TModel> ExecuteData<TModel, TArguments>(NpgsqlConnection connection,
            Func<TArguments, string> buildQueryDelegate,
            TArguments delegateArgsModel,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default) where TModel : class;
    }
}