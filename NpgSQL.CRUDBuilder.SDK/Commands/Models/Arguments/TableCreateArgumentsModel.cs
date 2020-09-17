using Npgsql;
using NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments.Interfaces;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments
{
    internal class TableCreateArgumentsModel : TransactionArgumentsModel, ITransactionArgumentsModel
    {
        public TableCreateArgumentsModel(NpgsqlConnection connection) : base(connection)
        {
            
        }
        
        internal 
    }
}