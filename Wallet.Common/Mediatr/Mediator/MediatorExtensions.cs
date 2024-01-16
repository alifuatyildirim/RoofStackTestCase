using System.Reflection;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Common.Mediatr.Command;
using Wallet.Common.Mediatr.Mediator.Processors;
using Wallet.Common.Mediatr.Query;

namespace Wallet.Common.Mediatr.Mediator
{
   public static class MediatorExtensions
    {
        public static IServiceCollection AddCqrsMediator(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            List<Assembly> enumerable = assemblies.Distinct().ToList();
            
            services.AddMediatr(enumerable);
            services.AddApplicationHandlers(enumerable);
            services.AddDomainHandlers(enumerable);
            services.AddValidators(enumerable);

            services.AddScoped<IApplicationCommandSender, CommandQueryMediator>();
            services.AddScoped<IDomainCommandSender, CommandQueryMediator>();
            services.AddScoped<IQueryProcessor, CommandQueryMediator>();
            
            return services;
        }

        private static void AddValidators(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            static bool Expression(Type type) => type.Is(typeof(IValidator<>));

            foreach (Type v in assemblies.SelectMany(x => x.GetTypes().Where(type => type.GetInterfaces().Any(Expression))))
            {
                foreach (Type i in v.GetInterfaces().Where(Expression))
                {
                    services.AddScoped(i, v);
                }
            }
        }

        private static void AddMediatr(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddMediatR(assemblies.ToArray())
                    .AddScoped(typeof(IRequestPreProcessor<>), typeof(ValidationRequestPreProcessor<>));
        }

        private static void AddApplicationHandlers(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            static bool Expression(Type type)
            {
                return !type.IsAbstract 
                       && (type.Is(typeof(IApplicationCommandHandler<,>))
                       || type.Is(typeof(IApplicationCommandHandler<>))
                       || type.Is(typeof(IQueryHandler<,>)));
            }

            foreach (Type h in assemblies.SelectMany(x => x.GetTypes().Where(type => type.GetInterfaces().Any(Expression))))
            {
                foreach (Type hi in h.GetInterfaces().Where(Expression))
                {
                    services.AddScoped(hi, h);
                }
            }
        }
        
        private static void AddDomainHandlers(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            static bool Expression(Type type)
            {
                return !type.IsAbstract
                       && type.Is(typeof(IDomainCommandHandler<>));
            }

            foreach (Type h in assemblies.SelectMany(x => x.GetTypes().Where(type => type.GetInterfaces().Any(Expression))))
            {
                foreach (Type hi in h.GetInterfaces().Where(Expression))
                {
                    services.AddScoped(hi, h);
                }
            }
        }

        private static bool Is(this Type type, Type typeCompare)
        {
            return type.IsGenericType && (type.Name.Equals(typeCompare.Name, StringComparison.InvariantCulture) || type.GetGenericTypeDefinition() == typeCompare);
        }
    }
}
