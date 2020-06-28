using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.QueryBuilders;

namespace NpgSQL.CRUDBuilder
{
    public struct CrudFunction
    {
        public async Task CreateDatabase(NpgsqlConnection npgsqlConnection, string dbLayout, string dbOwner = null,
            string dbCollactionEncoding = null, CancellationToken cancellationToken = default)
        {
            var queryBuilder = new NewDbQueryBuilder();
            await queryBuilder.ProcedureNewDbTransaction(npgsqlConnection, dbLayout, dbOwner, dbCollactionEncoding,
                cancellationToken);
        }
    }
}