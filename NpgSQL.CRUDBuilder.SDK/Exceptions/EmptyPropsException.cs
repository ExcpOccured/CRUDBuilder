using System;

namespace NpgSQL.CRUDBuilder.SDK.Exceptions
{
    internal class EmptyPropsException : Exception
    {
        private const string ExceptionMessage = "The type must contain internal properties!";

        internal EmptyPropsException() : base(ExceptionMessage) { }
    }
}