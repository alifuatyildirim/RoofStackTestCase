using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Application.Services.Abstraction;

namespace Wallet.Application.Extensions.ServiceRegistration
{
    public static class ApplicationServicesCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddServices();
            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            Assembly assm = Assembly.GetExecutingAssembly();
            static bool Expression(Type type) => typeof(IApplicationService).IsAssignableFrom(type);

            foreach (Type v in assm.GetTypes().Where(type => !type.IsInterface && Expression(type) && type.GetInterfaces().Any(Expression)))
            {
                foreach (Type i in v.GetInterfaces().Where(Expression))
                {
                    services.AddScoped(i, v);
                }
            }
        }
    }
}
