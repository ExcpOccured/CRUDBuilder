using System;

namespace NpgSQL.CRUDBuilder.Domain.Exceptions
{
    public class InvalidTransactionArgumentException : Exception
    {
        private const string ExceptionMessage = "Error validating transaction parameters!";

        public InvalidTransactionArgumentException() : base(ExceptionMessage) { }
    }
}