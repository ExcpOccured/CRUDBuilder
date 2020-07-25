using Npgsql;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments
{
    internal abstract class TransactionArgumentsModel
    {
        public NpgsqlConnection NpgsqlConnection { get; set; }
    }
}