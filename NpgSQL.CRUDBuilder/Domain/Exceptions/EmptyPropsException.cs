using System;

namespace NpgSQL.CRUDBuilder.Domain.Exceptions
{
    public class EmptyPropsException : Exception
    {
        private const string ExceptionMessage = "The type must contain public properties!";

        public EmptyPropsException() : base(ExceptionMessage) { }
    }
}