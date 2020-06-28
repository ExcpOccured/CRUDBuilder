using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;

namespace NpgSQL.CRUDBuilder.QueryBuilders.Interfaces
{
    public interface IQueryBuilder
    {
        bool ValidateTransactionArguments(ITransactionArgumentsModel model);

        string BuildQuery(ITransactionArgumentsModel model);
    }
}