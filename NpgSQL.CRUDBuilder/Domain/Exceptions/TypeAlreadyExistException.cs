using System;

namespace NpgSQL.CRUDBuilder.Domain.Exceptions
{
    internal class TypeAlreadyExistException : Exception
    {
        private const string ExceptionMessage = "Type with specified name already exist!";
        private static string BuildExceptionMessage(string typeLabel) => $"Type with specified name ({typeLabel}) already exist!"; 
        
        internal TypeAlreadyExistException() : base(ExceptionMessage) { }

        internal TypeAlreadyExistException(string typeLabel) : base(BuildExceptionMessage(typeLabel)) { }
    }
}