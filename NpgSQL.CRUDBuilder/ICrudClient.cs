using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace NpgSQL.CRUDBuilder
{
    public interface ICrudClient
    {
        Task CreateDatabase(NpgsqlConnection sqlConnection,
            string dbLayout,
            string dbOwner = null,
            string dbCollactionEncoding = null,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default);

        Task CreateDatabases(NpgsqlConnection sqlConnection,
            IEnumerable<string> dbLayouts,
            string dbOwner = null,
            string dbCollactionEncoding = null,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default);

        Task CreateTable<TTable>(NpgsqlConnection sqlConnection, TTable table,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default) where TTable : class;

        Task CreateTables<TTable>(NpgsqlConnection sqlConnection, IEnumerable<TTable> tables,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default) where TTable : class;
    }
}