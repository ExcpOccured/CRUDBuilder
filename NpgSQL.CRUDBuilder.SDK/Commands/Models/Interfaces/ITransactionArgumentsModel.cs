using Npgsql;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Interfaces
{
    internal interface ITransactionArgumentsModel
    {
        NpgsqlConnection NpgsqlConnection { get; }
    }
}