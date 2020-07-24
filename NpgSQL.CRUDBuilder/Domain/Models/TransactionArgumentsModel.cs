using Npgsql;
using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;

namespace NpgSQL.CRUDBuilder.Domain.Models
{
    internal abstract class TransactionArgumentsModel : ITransactionArgumentsModel
    {
        internal NpgsqlConnection NpgsqlConnection { get; set; }
    }
}