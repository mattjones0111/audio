namespace Process.DependencyResolution
{
    using System.Reflection;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseFeatures(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.Decorate<IControllerFactory, MediatorProviderControllerFactoryDecorator>();
            
            return services;
        }
    }
}
