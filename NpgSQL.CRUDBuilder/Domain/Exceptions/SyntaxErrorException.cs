using System;

namespace NpgSQL.CRUDBuilder.Domain.Exceptions
{
    public class SyntaxErrorException : Exception
    {
        private static string BuildExceptionMessage(string typeLabel) =>
            $"Type with specified name ({typeLabel}) already exist!";

        public SyntaxErrorException()
        {
        }
    }
}