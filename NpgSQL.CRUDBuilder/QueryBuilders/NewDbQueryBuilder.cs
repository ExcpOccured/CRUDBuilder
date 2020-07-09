using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using NpgSQL.CRUDBuilder.Domain.Extensions;
using NpgSQL.CRUDBuilder.Domain.Models;
using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;
using NpgSQL.CRUDBuilder.Domain.Providers;
using NpgSQL.CRUDBuilder.QueryBuilders.Interfaces;

namespace NpgSQL.CRUDBuilder.QueryBuilders
{
    public class NewDbQueryBuilder : IQueryBuilder
    {
        private const string DefaultDbOwner = "postgres";

        private const string DefaultDbEncoding = "UTF-8";

        public async Task ProcedureNewDbTransaction(NpgsqlConnection npgsqlConnection, 
            string dbLayout,
            string dbOwner = null,
            string dbCollactionEncoding = null,
            CancellationToken cancellationToken = default)
        {
            var transactionProvider = new DataOverTransactionProvider();

            var argumentsModel = new DbCreateArgumentsModel
            {
                DbCollationEncoding = dbCollactionEncoding,
                DbLayout = dbLayout,
                DbOwner = dbOwner,
                NpgsqlConnection = npgsqlConnection
            };

            await transactionProvider.ExecuteNonQuery(npgsqlConnection, BuildQuery, argumentsModel,
                ValidateTransactionArguments, cancellationToken: cancellationToken);
        }

        public bool ValidateTransactionArguments(ITransactionArgumentsModel model)
        {
            var dbCreateArgumentsModel = (DbCreateArgumentsModel) model;

            if (dbCreateArgumentsModel.NpgsqlConnection is null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(dbCreateArgumentsModel.DbLayout))
            {
                return false;
            }

            if (dbCreateArgumentsModel.DbOwner != null)
            {
                if (string.IsNullOrEmpty(dbCreateArgumentsModel.DbOwner))
                {
                    return false;
                }
            }

            if (!(dbCreateArgumentsModel.DbCollationEncoding is null) &&
                string.IsNullOrEmpty(dbCreateArgumentsModel.DbCollationEncoding))
            {
                return false;
            }

            return true;
        }

        public string BuildQuery(ITransactionArgumentsModel model)
        {
            var dbCreateArgumentsModel = (DbCreateArgumentsModel) model;
            var queryBuilder = new StringBuilder();

            var dbOwner = string.IsNullOrEmpty(dbCreateArgumentsModel.DbOwner)
                ? DefaultDbOwner
                : dbCreateArgumentsModel.DbOwner;

            var dbEncoding = string.IsNullOrEmpty(dbCreateArgumentsModel.DbCollationEncoding)
                ? DefaultDbEncoding
                : dbCreateArgumentsModel.DbCollationEncoding;

            var localeAliasPrefix = Thread.CurrentThread.CurrentCulture.Name.AdaptLocaleAlias();
            var lcTypeString = $"'{localeAliasPrefix}.{dbEncoding}'";

            queryBuilder.AppendLine($"CREATE DATABASE {dbCreateArgumentsModel.DbLayout}");
            queryBuilder.AppendLine("WITH");
            queryBuilder.AppendLine($"OWNER = {dbOwner}");
            queryBuilder.AppendLine($"LC_COLLATE = {lcTypeString}");
            queryBuilder.AppendLine($"LC_CTYPE = {lcTypeString}");
            queryBuilder.AppendLine("TABLESPACE = pg_default");
            queryBuilder.AppendLine("CONNECTION LIMIT = -1;");

            return queryBuilder.ToString();
        }
    }
}