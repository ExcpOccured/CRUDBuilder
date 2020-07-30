using System.ComponentModel.DataAnnotations;

namespace NpgSQL.CRUDBuilder.NpgSQLTypes
{
    public enum NpgSqlType
    {
        [Display(Name = "boolean")]
        Boolean,
        
        [Display(Name = "smallint")]
        Smallint,
        
        [Display(Name = "integer")]
        Integer,
        
        [Display(Name = "real")]
        Float,
        
        [Display(Name = "double precision")]
        Double,
        
        [Display(Name = "money")]
        Money,
        
        [Display(Name = "text")]
        Text,
        
        [Display(Name = "character")]
        CharacterVarying,
        
        [Display(Name = "bit varying")]
        BitVarying,
        
        [Display(Name = "interval")]
        Interval,
        
        [Display(Name = "timestamp without time zone")]
        TimestampWithoutTimeZone,
        
        [Display(Name = "timestamp with time zone")]
        TimestampWithTimeZone,
        
        [Display(Name = "bytea")]
        Bytea,
        
        [Display(Name = "json")]
        Json
    }
}