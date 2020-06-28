namespace NpgSQL.CRUDBuilder.Domain.Models
{
    public class DbCreateArgumentsModel : TransactionArgumentsModel
    {
        public string DbLayout { get; set; }

        public string DbOwner { get; set; }

        public string DbCollationEncoding { get; set; }
    }
}