using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace NpgSQL.CRUDBuilder.SDK.Extensions
{
    internal static class EnumExtensions<TValue>
    {
        public static IList<TValue> GetValues(Enum enumType)
        {
            var calculatedEnumType = enumType.GetType();

            return calculatedEnumType.GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Select(fieldInfo => (TValue) Enum.Parse(calculatedEnumType, fieldInfo.Name, false)).ToList();
        }
        
        public static TValue Parse(string value)
        {
            return (TValue)Enum.Parse(typeof(TValue), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        private static string LookupResource(IReflect resourceManagerProvider, string resourceKey)
        {
            foreach (var staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType != typeof(System.Resources.ResourceManager)) continue;
                var resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                return resourceManager?.GetString(resourceKey);
            }

            return resourceKey;
        }

        public static string GetDisplayValue(TValue value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString() ?? string.Empty);

            var descriptionAttributes = fieldInfo?.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes?[0].ResourceType != null)
                return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            if (descriptionAttributes == null) return string.Empty;
            
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
    }
}