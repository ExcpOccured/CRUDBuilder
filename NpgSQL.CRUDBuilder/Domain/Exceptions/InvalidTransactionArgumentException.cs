using System;

namespace NpgSQL.CRUDBuilder.Domain.Exceptions
{
    internal class InvalidTransactionArgumentException : Exception
    {
        private const string ExceptionMessage = "Error validating transaction parameters!";

        internal InvalidTransactionArgumentException() : base(ExceptionMessage) { }
    }
}