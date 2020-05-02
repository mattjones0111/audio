namespace Integration.Tests
{
    using System;
    using Adapter.Azure.DependencyResolution;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Process.DependencyResolution;

    public class IntegrationTest
    {
        protected Func<IMediator> Mediator { get; }

        public IntegrationTest()
        {
            IServiceCollection services = new ServiceCollection();

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton(config);

            services.UseFeatures();

            services.UseAzureDocumentStorage(
                "UseDevelopmentStorage=true;",
                "documents");

            IServiceProvider provider = services.BuildServiceProvider();

            Mediator = () => provider.GetService<IMediator>();
        }
    }
}
