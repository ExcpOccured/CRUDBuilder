using System;
using Npgsql;

namespace NpgSQL.CRUDBuilder.Domain.Mapping
{
    internal static class DtoPropsMapper
    {
        public static TData TryMapDtoProps<TData>(NpgsqlDataReader reader)
            where TData : class
        {
            throw new NotImplementedException();
        }
    }
}