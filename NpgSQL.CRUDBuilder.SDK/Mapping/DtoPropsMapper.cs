using System;
using Npgsql;

namespace NpgSQL.CRUDBuilder.SDK.Mapping
{
    internal static class DtoPropsMapper
    {
        internal static TData TryMapDtoProps<TData>(NpgsqlDataReader reader)
            where TData : class
        {
            throw new NotImplementedException();
        }
    }
}