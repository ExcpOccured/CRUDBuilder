#nullable enable

using System;
using Npgsql;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Transaction
{
    internal readonly struct TransactionResult
    {
        internal readonly TransactionResultState State;

        internal readonly Exception? Exception;

        internal readonly NpgsqlDataReader? DataReader;

        internal TransactionResult(TransactionResultState state, Exception? exception = null,
            NpgsqlDataReader npgsqlDataReader = null)
        {
            State = state;
            Exception = exception;
            DataReader = npgsqlDataReader;
        }
    }
}