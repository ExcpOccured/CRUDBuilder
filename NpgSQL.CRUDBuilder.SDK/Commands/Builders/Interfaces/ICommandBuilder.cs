using NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments.Interfaces;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Builders.Interfaces
{
    internal interface ICommandBuilder<out TTransactionArgumentsModel>
        where TTransactionArgumentsModel : ITransactionArgumentsModel
    {
        TTransactionArgumentsModel ArgumentsModel { get; }

        bool ValidateQueryArgumentsModel();

        string BuildQuery();
    }
}