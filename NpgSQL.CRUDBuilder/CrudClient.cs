using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.Interfaces;
using NpgSQL.CRUDBuilder.SDK.Commands.Builders;
using NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments;
using NpgSQL.CRUDBuilder.SDK.QueryBuilders;

namespace NpgSQL.CRUDBuilder
{
    public class CrudClient : NpgSqlClient, ICrudClient
    {
        public async Task CreateDatabase(NpgsqlConnection sqlConnection,
            string dbLayout,
            string dbOwner = null,
            string encoding = null,
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default)
        {
            var queryBuilder = new TransactionQueryBuilder();

            var argsModel = new DbCreateArgumentsModel(sqlConnection, dbLayout, dbOwner,
                encoding);

            var commandBuilder = new DbCreationCommandBuilder(argsModel);

            var query =
                queryBuilder.CompileTransactionExpression(
                    commandBuilder);

            await commandBuilder.ExecuteNonQuery(sqlConnection, query, keepConnectionOpen, cancellationToken);
        }

        public Task CreateTable<TTable>(NpgsqlConnection sqlConnection, TTable table, bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default) where TTable : class
        {
            throw new NotImplementedException();
        }
    }
}