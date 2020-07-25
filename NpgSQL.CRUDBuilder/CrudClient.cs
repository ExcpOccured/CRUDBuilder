using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.SDK.Commands.Builders;
using NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments;
using NpgSQL.CRUDBuilder.SDK.QueryBuilders;

namespace NpgSQL.CRUDBuilder
{
    public class CrudClient
    {
        public async Task CreateDatabase(NpgsqlConnection sqlConnection,
            string dbLayout,
            string dbOwner = null,
            string dbCollactionEncoding = null,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default)
        {
            var queryBuilder = new TransactionQueryBuilder();

            var argsModel = new DbCreateArgumentsModel(sqlConnection, dbLayout, dbOwner,
                dbCollactionEncoding);

            var commandBuilder = new DbCreationCommandBuilder(argsModel);

            var query =
                queryBuilder.CompileTransactionExpression<DbCreateArgumentsModel, DbCreationCommandBuilder>(
                    commandBuilder);

            await commandBuilder.ExecuteNonQuery(sqlConnection, query, keepConnectionOpen, cancellationToken);
        }

        public async Task CreateDatabases(NpgsqlConnection sqlConnection,
            IEnumerable<string> dbLayouts,
            string dbOwner = null,
            string dbCollactionEncoding = null,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default)
        {
            foreach (var dbLayout in dbLayouts)
            {
                await CreateDatabase(sqlConnection, dbLayout, dbOwner, dbCollactionEncoding, keepConnectionOpen,
                    cancellationToken);
            }
        }
    }
}