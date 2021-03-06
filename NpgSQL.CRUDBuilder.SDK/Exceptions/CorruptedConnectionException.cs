using System;

namespace NpgSQL.CRUDBuilder.SDK.Exceptions
{
    internal class CorruptedConnectionException : Exception

    {
        private const string ExceptionMessage =
            "To perform a transaction, must have an ConnectionState.Closed or ConnectionState.Closed";

        internal CorruptedConnectionException() : base(ExceptionMessage)
        {
        }
    }
}