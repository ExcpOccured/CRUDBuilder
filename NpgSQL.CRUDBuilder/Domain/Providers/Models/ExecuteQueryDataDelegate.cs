using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace NpgSQL.CRUDBuilder.Domain.Providers.Models
{
    internal delegate Task<NpgsqlDataReader> ExecuteQueryDataDelegate(CancellationToken cancellationToken = default);
}