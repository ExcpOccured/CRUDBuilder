using System;

namespace NpgSQL.CRUDBuilder.Domain.Exceptions
{
    public class InnerException : Exception
    {
        private const string ExceptionMessage = "An unhandled exception occurred in NpgSQL.CRUDBuilder!"; 
        
        public InnerException(string exceptionMessage = null) : base(string.IsNullOrEmpty(exceptionMessage) 
            ? ExceptionMessage : exceptionMessage) { }
    }
}