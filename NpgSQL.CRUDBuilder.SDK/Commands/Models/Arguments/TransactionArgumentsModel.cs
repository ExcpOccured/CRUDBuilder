using Npgsql;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments
{
    internal abstract class TransactionArgumentsModel
    {
        public NpgsqlConnection Connection { get; }

        protected TransactionArgumentsModel(NpgsqlConnection connection)
        {
            Connection = connection;
        }
    }
}