using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using NpgSQL.CRUDBuilder.SDK.Commands.Builders.Interfaces;
using NpgSQL.CRUDBuilder.SDK.Commands.Models.Arguments.Interfaces;
using NpgSQL.CRUDBuilder.SDK.Exceptions;

[assembly: InternalsVisibleTo("NpgSQL.CRUDBuilder")]

namespace NpgSQL.CRUDBuilder.SDK.QueryBuilders
{
    internal class TransactionQueryBuilder
    {
        internal string CompileTransactionExpression<TTransactionArgumentsModel, TCommandBuilder>(
            TCommandBuilder commandBuilder,
            Expression<Func<TTransactionArgumentsModel, string>> expression = null)
            where TTransactionArgumentsModel : ITransactionArgumentsModel
            where TCommandBuilder : ICommandBuilder<ITransactionArgumentsModel>
        {
            if (!commandBuilder.ValidateQueryArgumentsModel())
            {
                throw new InvalidTransactionArgumentException();
            }

            return commandBuilder.BuildQuery();
        }
    }
}