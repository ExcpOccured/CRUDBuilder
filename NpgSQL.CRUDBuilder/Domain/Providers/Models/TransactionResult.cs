#nullable enable

using System;
using Npgsql;

namespace NpgSQL.CRUDBuilder.Domain.Providers.Models
{
    public readonly struct TransactionResult
    {
        public readonly TransactionResultState State;

        public readonly Exception? Exception;

        public readonly NpgsqlDataReader? DataReader;

        public TransactionResult(TransactionResultState state, Exception? exception = null,
            NpgsqlDataReader npgsqlDataReader = null)
        {
            State = state;
            Exception = exception;
            DataReader = npgsqlDataReader;
        }
    }
}