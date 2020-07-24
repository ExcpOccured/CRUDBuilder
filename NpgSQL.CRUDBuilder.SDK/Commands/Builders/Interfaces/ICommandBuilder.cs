using NpgSQL.CRUDBuilder.SDK.Commands.Models.Interfaces;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Builders.Interfaces
{
    internal interface ICommandBuilder<in TTransactionModel>
        where TTransactionModel : ITransactionArgumentsModel
    {
        bool ValidateQueryArgumentsModel(TTransactionModel argumentsModel);

        string BuildQuery(TTransactionModel argumentsModel);
    }
}