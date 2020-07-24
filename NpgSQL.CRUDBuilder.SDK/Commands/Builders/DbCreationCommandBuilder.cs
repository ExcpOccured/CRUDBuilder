using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using NpgSQL.CRUDBuilder.SDK.Commands.Builders.Interfaces;
using NpgSQL.CRUDBuilder.SDK.Commands.Models;
using NpgSQL.CRUDBuilder.SDK.Extensions;

[assembly: InternalsVisibleTo("NpgSQL.CRUDBuilder")]

namespace NpgSQL.CRUDBuilder.SDK.Commands.Builders
{
    internal class DbCreationCommandBuilder : ICommandBuilder<DbCreateArgumentsModel>
    {
        private const string DefaultDbOwner = "postgres";

        private const string DefaultDbEncoding = "UTF-8";

        public bool ValidateQueryArgumentsModel(DbCreateArgumentsModel argumentsModel)
        {
            if (argumentsModel.NpgsqlConnection is null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(argumentsModel.DbLayout))
            {
                return false;
            }

            if (argumentsModel.DbOwner != null)
            {
                if (string.IsNullOrEmpty(argumentsModel.DbOwner))
                {
                    return false;
                }
            }

            if (!(argumentsModel.DbCollationEncoding is null) &&
                string.IsNullOrEmpty(argumentsModel.DbCollationEncoding))
            {
                return false;
            }

            return true;
        }

        public string BuildQuery(DbCreateArgumentsModel argumentsModel)
        {
            var queryBuilder = new StringBuilder();

            var dbOwner = string.IsNullOrEmpty(argumentsModel.DbOwner)
                ? DefaultDbOwner
                : argumentsModel.DbOwner;

            var dbEncoding = string.IsNullOrEmpty(argumentsModel.DbCollationEncoding)
                ? DefaultDbEncoding
                : argumentsModel.DbCollationEncoding;

            var localeAliasPrefix = Thread.CurrentThread.CurrentCulture.Name.AdaptLocaleAlias();
            var lcTypeString = $"'{localeAliasPrefix}.{dbEncoding}'";

            queryBuilder.AppendLine($"CREATE DATABASE {argumentsModel.DbLayout}");
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