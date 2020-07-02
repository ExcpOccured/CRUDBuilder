#nullable enable

using System;
using System.Threading.Tasks;

namespace NpgSQL.CRUDBuilder.Domain.Providers.Models
{
    internal readonly struct TransactionDelegate
    {
        public readonly Func<Task> NonQueryDelegate;

        public readonly ExecuteQueryDataDelegate? QueryWithDataDelegate;

        public TransactionDelegate(Func<Task> nonQueryDelegate,
            ExecuteQueryDataDelegate queryWithDataDelegate = null)
        {
            NonQueryDelegate = nonQueryDelegate;
            QueryWithDataDelegate = queryWithDataDelegate;
        }
    }
}