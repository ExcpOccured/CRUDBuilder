using System;
using System.Linq;
using System.Reflection;
using NpgSQL.CRUDBuilder.Attributes;
using NpgSQL.CRUDBuilder.Domain.Exceptions;

namespace NpgSQL.CRUDBuilder.Domain.StringSearchAlgorithms
{
    internal class ReflectionTypePropsAggregator
    {
        public string[] MakeTypePropsArray(Type type, bool useAttributePolicy)
        {
            var typeProps = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToList();

            if (!typeProps.Any())
            {
                throw new EmptyPropsException();
            }

            if (!useAttributePolicy) return typeProps.Select(info => info.ToString()).ToArray();
            {
                var notMappedProps =
                    typeProps.Where(info => info.GetCustomAttribute<NotMappingAttribute>() == null).ToList();

                return !notMappedProps.Any() 
                    ? new string[] { } 
                    : notMappedProps.Select(info => info.ToString()).ToArray();
            }
        }
    }
}