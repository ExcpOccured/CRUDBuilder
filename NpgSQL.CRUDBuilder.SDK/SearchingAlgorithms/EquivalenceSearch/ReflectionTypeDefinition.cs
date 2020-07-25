using System;
using System.Collections.Generic;
using System.Reflection;

namespace NpgSQL.CRUDBuilder.SDK.SearchingAlgorithms.EquivalenceSearch
{
    //TODO:Implement assemblies triangle
    internal class ReflectionTypeDefinition
    {
        internal List<string> GetTypePropsList(Type type, bool useAttributePolicy)
        {
            /*var typeProps = GetTypeProperties(type);

            if (useAttributePolicy)
            {
                return typeProps.Where(info => info.GetCustomAttribute<NotMappingAttribute>() == null)
                    .Select(info => info.Name)
                    .ToList();
            }

            return typeProps.Select(info => info.Name).ToList();*/
            
            throw new NotImplementedException();
        }

        internal List<PropertyInfo> GetTypePropsStringList(Type type, bool useAttributePolicy)
        {
            /*var typeProps = GetTypeProperties(type);

            if (useAttributePolicy)
            {
                return typeProps.Where(info => info.GetCustomAttribute<NotMappingAttribute>() == null)
                    .ToList();
            }

            return typeProps.ToList();
        }

        private IEnumerable<PropertyInfo> GetTypeProperties(IReflect type)
        {
            var typeProps = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToList();

            if (!typeProps.Any())
            {
                throw new EmptyPropsException();
            }

            return typeProps;*/
            
            throw new NotImplementedException();
        }
    }
}