using Npgsql;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments.Interfaces
{
    internal interface ITransactionArgumentsModel
    {
        NpgsqlConnection NpgsqlConnection { get; }
    }
}