#nullable enable

using System;
using Npgsql;

namespace NpgSQL.CRUDBuilder.Domain.Providers.Models
{
    public readonly struct TransactionResult
    {
        private readonly TransactionResultState State;

        private readonly Exception? Exception;

        private readonly NpgsqlDataReader? DataReader;

        public TransactionResult(TransactionResultState state, Exception? exception = null, 
            NpgsqlDataReader npgsqlDataReader = null)
        {
            State = state;
            Exception = exception;
            DataReader = npgsqlDataReader;
        }
    }
}