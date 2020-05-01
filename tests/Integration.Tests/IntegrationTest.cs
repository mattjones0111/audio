namespace Integration.Tests
{
    using System;
    using Adapter.Azure;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Process.DependencyResolution;
    using Process.Ports;

    public class IntegrationTest
    {
        protected Func<IMediator> Mediator { get; }

        public IntegrationTest()
        {
            IServiceCollection services = new ServiceCollection();

            services.UseFeatures();

            services.AddTransient<IStoreDocuments, DocumentStore>();

            IServiceProvider provider = services.BuildServiceProvider();

            Mediator = () => provider.GetService<IMediator>();
        }
    }
}