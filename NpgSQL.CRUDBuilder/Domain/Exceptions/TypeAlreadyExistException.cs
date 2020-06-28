using System;

namespace NpgSQL.CRUDBuilder.Domain.Exceptions
{
    public class TypeAlreadyExistException : Exception
    {
        private const string ExceptionMessage = "Type with specified name already exist!";
        private static string BuildExceptionMessage(string typeLabel) => $"Type with specified name ({typeLabel}) already exist!"; 
        
        public TypeAlreadyExistException() : base(ExceptionMessage) { }

        public TypeAlreadyExistException(string typeLabel) : base(BuildExceptionMessage(typeLabel)) { }
    }
}