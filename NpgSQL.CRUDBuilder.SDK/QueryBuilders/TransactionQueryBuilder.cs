using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using NpgSQL.CRUDBuilder.SDK.Commands.Builders.Interfaces;
using NpgSQL.CRUDBuilder.SDK.Commands.Models.Interfaces;
using NpgSQL.CRUDBuilder.SDK.Exceptions;

[assembly: InternalsVisibleTo("NpgSQL.CRUDBuilder")]

namespace NpgSQL.CRUDBuilder.SDK.QueryBuilders
{
    internal class TransactionQueryBuilder
    {
        internal string CompileTransactionExpression<TTransactionModel, TCommandBuilder>(
            TCommandBuilder commandBuilder,
            Func<TTransactionModel, bool> validatePredicate,
            Expression<Func<TTransactionModel, string>> expression = null)
            where TTransactionModel : ITransactionArgumentsModel
            where TCommandBuilder : ICommandBuilder<ITransactionArgumentsModel>
        {
            if (!validatePredicate(model))
            {
                throw new InvalidTransactionArgumentException();
            }

            return commandBuilder.BuildQuery(model);
        }
    }
}