using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Transaction
{
    internal delegate Task<NpgsqlDataReader> ExecuteQueryDataDelegate(CancellationToken cancellationToken = default);
}