using System;
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
            Func<string> expression = null)
            where TTransactionArgumentsModel : ITransactionArgumentsModel
            where TCommandBuilder : ICommandBuilder<ITransactionArgumentsModel>
        {
            if (!(expression is null))
            {
                return expression();
            }

            if (!commandBuilder.ValidateQueryArgumentsModel())
            {
                throw new InvalidTransactionArgumentException();
            }

            return commandBuilder.BuildQuery();
        }
    }
}