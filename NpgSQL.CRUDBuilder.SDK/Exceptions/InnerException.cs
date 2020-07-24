using System;

namespace NpgSQL.CRUDBuilder.SDK.Exceptions
{
    internal class InnerException : Exception
    {
        private const string ExceptionMessage = "An unhandled exception occurred in NpgSQL.CRUDBuilder!"; 
        
        internal InnerException(string exceptionMessage = null) : base(string.IsNullOrEmpty(exceptionMessage) 
            ? ExceptionMessage : exceptionMessage) { }
    }
}