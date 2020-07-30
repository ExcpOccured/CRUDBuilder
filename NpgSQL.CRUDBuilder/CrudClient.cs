using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.SDK.Commands.Builders;
using NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments;
using NpgSQL.CRUDBuilder.SDK.QueryBuilders;

namespace NpgSQL.CRUDBuilder
{
    public class CrudClient : ICrudClient
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
                await CreateDatabase(sqlConnection, dbLayout, dbOwner, dbCollactionEncoding, true,
                    cancellationToken);
            }

            if (!keepConnectionOpen)
            {
                await new TransactionDispatcherBuilder().TryToDisposeConnection(sqlConnection);
            }
        }

        public Task CreateTable<TTable>(NpgsqlConnection sqlConnection, TTable table, bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default) where TTable : class
        {
            throw new NotImplementedException();
        }

        public async Task CreateTables<TTable>(NpgsqlConnection sqlConnection, IEnumerable<TTable> tables, 
            bool keepConnectionOpen = true,
            CancellationToken cancellationToken = default) where TTable : class
        {
            foreach (var table in tables)
            {
                await CreateTable(sqlConnection, table, true, cancellationToken);
            }
            
            if (!keepConnectionOpen)
            {
                await new TransactionDispatcherBuilder().TryToDisposeConnection(sqlConnection);
            }
        }
    }
}