using Npgsql;
using NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments.Interfaces;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments
{
    internal class DbCreateArgumentsModel : TransactionArgumentsModel, ITransactionArgumentsModel
    {
        private const string DefaultDbOwner = "postgres";

        private const string DefaultDbEncoding = "UTF-8";

        public DbCreateArgumentsModel(NpgsqlConnection connection, string dbLayout, string dbOwner,
            string dbCollationEncoding) : base(connection)
        {
            DbLayout = dbLayout;
            
            DbOwner = string.IsNullOrEmpty(dbOwner) 
                ? DefaultDbOwner
                : dbOwner;
            
            DbCollationEncoding = string.IsNullOrEmpty(dbCollationEncoding) 
                ? DefaultDbEncoding 
                : dbCollationEncoding;
        }

        internal string DbLayout { get; }

        internal string DbOwner { get; }

        internal string DbCollationEncoding { get; }
    }
}