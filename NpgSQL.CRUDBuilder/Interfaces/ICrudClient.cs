using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace NpgSQL.CRUDBuilder.Interfaces
{
    public interface ICrudClient : INpgSqlClient
    {
        Task CreateDatabase(NpgsqlConnection sqlConnection,
            string dbLayout,
            string dbOwner = null,
            string encoding = null,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default);

        Task CreateTable<TTable>(NpgsqlConnection sqlConnection, TTable table,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default) where TTable : class;
    }
}