using Npgsql;
using NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments.Interfaces;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments
{
    internal class DbCreateArgumentsModel : TransactionArgumentsModel, ITransactionArgumentsModel
    {
        private const string DefaultDbOwner = "postgres";

        private const string DefaultDbEncoding = "UTF-8";

        public DbCreateArgumentsModel(NpgsqlConnection npgsqlConnection, string dbLayout, string dbOwner,
            string dbCollationEncoding)
        {
            DbLayout = dbLayout;
            DbOwner = dbOwner ?? DefaultDbOwner;
            DbCollationEncoding = dbCollationEncoding ?? DefaultDbEncoding;
            NpgsqlConnection = npgsqlConnection;
        }

        internal string DbLayout { get; }

        internal string DbOwner { get; }

        internal string DbCollationEncoding { get; }
    }
}