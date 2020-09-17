using System;

namespace NpgSQL.CRUDBuilder.NpgSQLTypes.Attributes
{
    public class NpgSqlDataTypeAttribute : Attribute
    {
        private readonly NpgSqlType _sqlType;

        private readonly int? _length;
        
        public NpgSqlDataTypeAttribute(NpgSqlType sqlType, int? length = null)
        {
            _sqlType = sqlType;
            _length = length;
        }
    }
}