using System;
using Microsoft.Extensions.DependencyInjection;
using NpgSQL.CRUDBuilder.Interfaces;

namespace NpgSQL.CRUDBuilder.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterCrudClient(this IServiceCollection serviceCollection, ServiceDescriptor lifeTime)
        {
            switch (lifeTime.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    serviceCollection.AddSingleton<ICrudClient, CrudClient>();
                    break;
                case ServiceLifetime.Scoped:
                    serviceCollection.AddScoped<ICrudClient, CrudClient>();
                    break;
                case ServiceLifetime.Transient:
                    serviceCollection.AddTransient<ICrudClient, CrudClient>();
                    break;
                default:
                    throw new NullReferenceException(nameof(lifeTime));
            }
        }
    }
}