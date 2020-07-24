using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;

namespace NpgSQL.CRUDBuilder.QueryBuilders.Interfaces
{
    internal interface IQueryBuilder
    {
        internal bool ValidateTransactionArguments(ITransactionArgumentsModel model);

        internal string BuildQuery(ITransactionArgumentsModel model);
    }
}