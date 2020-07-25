#nullable enable

using System;
using System.Threading.Tasks;

namespace NpgSQL.CRUDBuilder.SDK.Commands.Models.Transaction
{
    internal readonly struct TransactionDelegate
    {
        internal readonly Func<Task> NonQueryDelegate;

        internal readonly ExecuteQueryDataDelegate? QueryWithDataDelegate;

        internal TransactionDelegate(Func<Task> nonQueryDelegate,
            ExecuteQueryDataDelegate queryWithDataDelegate = null)
        {
            NonQueryDelegate = nonQueryDelegate;
            QueryWithDataDelegate = queryWithDataDelegate;
        }
    }
}