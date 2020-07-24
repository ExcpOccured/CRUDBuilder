using System;

namespace NpgSQL.CRUDBuilder.SDK.Exceptions
{
    internal class SyntaxErrorException : Exception
    {
        private static string BuildExceptionMessage(string typeLabel) =>
            $"Type with specified name ({typeLabel}) already exist!";

        internal SyntaxErrorException()
        {
        }
    }
}