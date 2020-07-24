namespace NpgSQL.CRUDBuilder.Domain.Models
{
    internal class DbCreateArgumentsModel : TransactionArgumentsModel
    {
        internal string DbLayout { get; set; }

        internal string DbOwner { get; set; }

        internal string DbCollationEncoding { get; set; }
    }
}