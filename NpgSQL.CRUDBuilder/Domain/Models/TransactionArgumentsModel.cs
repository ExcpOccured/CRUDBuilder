using Npgsql;
using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;

namespace NpgSQL.CRUDBuilder.Domain.Models
{
    public abstract class TransactionArgumentsModel : ITransactionArgumentsModel
    {
        public NpgsqlConnection NpgsqlConnection { get; set; }
    }
}