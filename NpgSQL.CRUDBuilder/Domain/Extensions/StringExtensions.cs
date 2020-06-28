using System.Text;

namespace NpgSQL.CRUDBuilder.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string AdaptLocaleAlias(this string cultureName)
        {
            var stringBuilder = new StringBuilder(cultureName) {[2] = '_'};
            return stringBuilder.ToString();
        }
    }
}