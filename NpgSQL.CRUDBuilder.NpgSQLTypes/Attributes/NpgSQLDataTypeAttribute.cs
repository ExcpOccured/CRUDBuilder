using System;

namespace NpgSQL.CRUDBuilder.NpgSQLTypes.Attributes
{
    public class NpgSqlDataTypeAttribute : Attribute
    {
        private readonly NpgSqlType _npgSqlType;

        private readonly int? _length;
        
        public NpgSqlDataTypeAttribute(NpgSqlType npgSqlType, int? length = null)
        {
            _npgSqlType = npgSqlType;
            _length = length;
        }
    }
}