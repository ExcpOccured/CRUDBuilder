using Npgsql;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models
{
    internal abstract class TransactionArgumentsModel
    {
        public NpgsqlConnection NpgsqlConnection { get; set; }
    }
}