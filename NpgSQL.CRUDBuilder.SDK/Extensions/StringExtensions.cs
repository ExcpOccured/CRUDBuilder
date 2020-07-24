using System.Text;

namespace NpgSQL.CRUDBuilder.SDK.Extensions
{
    internal static class StringExtensions
    {
        internal static string AdaptLocaleAlias(this string cultureName)
        {
            var stringBuilder = new StringBuilder(cultureName) {[2] = '_'};
            return stringBuilder.ToString();
        }
    }
}