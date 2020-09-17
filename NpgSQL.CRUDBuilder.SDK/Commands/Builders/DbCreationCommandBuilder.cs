using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using NpgSQL.CRUDBuilder.SDK.Commands.Builders.Interfaces;
using NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments;
using NpgSQL.CRUDBuilder.SDK.Extensions;

[assembly: InternalsVisibleTo("NpgSQL.CRUDBuilder")]

namespace NpgSQL.CRUDBuilder.SDK.Commands.Builders
{
    internal class DbCreationCommandBuilder : NpgSqlCrudCommand, ICommandBuilder<DbCreateArgumentsModel>
    {
        private const string DefaultDbOwner = "postgres";

        private const string DefaultDbEncoding = "UTF-8";

        public DbCreationCommandBuilder(DbCreateArgumentsModel argumentsModel)
        {
            ArgumentsModel = argumentsModel;
        }

        public DbCreateArgumentsModel ArgumentsModel { get; }

        public bool ValidateQueryArgumentsModel()
        {
            if (ArgumentsModel.Connection is null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(ArgumentsModel.DbLayout))
            {
                return false;
            }

            if (ArgumentsModel.DbOwner != null)
            {
                if (string.IsNullOrEmpty(ArgumentsModel.DbOwner))
                {
                    return false;
                }
            }

            if (!(ArgumentsModel.DbCollationEncoding is null) &&
                string.IsNullOrEmpty(ArgumentsModel.DbCollationEncoding))
            {
                return false;
            }

            return true;
        }

        public string BuildQuery()
        {
            var queryBuilder = new StringBuilder();

            var dbOwner = string.IsNullOrEmpty(ArgumentsModel.DbOwner)
                ? DefaultDbOwner
                : ArgumentsModel.DbOwner;

            var dbEncoding = string.IsNullOrEmpty(ArgumentsModel.DbCollationEncoding)
                ? DefaultDbEncoding
                : ArgumentsModel.DbCollationEncoding;

            var localeAliasPrefix = Thread.CurrentThread.CurrentCulture.Name.AdaptLocaleAlias();
            var lcTypeString = $"'{localeAliasPrefix}.{dbEncoding}'";

            queryBuilder.AppendLine($"CREATE DATABASE {ArgumentsModel.DbLayout}");
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