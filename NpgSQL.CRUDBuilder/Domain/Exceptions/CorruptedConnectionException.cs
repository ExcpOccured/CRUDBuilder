using System;

namespace NpgSQL.CRUDBuilder.Domain.Exceptions
{
    public class CorruptedConnectionException : Exception

    {
        private const string ExceptionMessage =
            "To perform a transaction, must have an ConnectionState.Closed or ConnectionState.Closed";

        public CorruptedConnectionException() : base(ExceptionMessage)
        {
        }
    }
}