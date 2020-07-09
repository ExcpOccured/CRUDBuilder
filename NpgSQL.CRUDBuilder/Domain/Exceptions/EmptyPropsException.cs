using System;

namespace NpgSQL.CRUDBuilder.Domain.Exceptions
{
    public class EmptyPropsException : Exception
    {
        private const string ExceptionMessage = "The type must contain open properties to be used as a table!";

        public EmptyPropsException() : base(ExceptionMessage) { }
    }
}