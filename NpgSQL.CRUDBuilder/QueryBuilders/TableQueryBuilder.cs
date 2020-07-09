using NpgSQL.CRUDBuilder.Domain.Models.Interfaces;
using NpgSQL.CRUDBuilder.QueryBuilders.Interfaces;

namespace NpgSQL.CRUDBuilder.QueryBuilders
{
    public class TableQueryBuilder : IQueryBuilder
    {
        public bool ValidateTransactionArguments(ITransactionArgumentsModel model)
        {
            throw new System.NotImplementedException();
        }

        public string BuildQuery(ITransactionArgumentsModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}