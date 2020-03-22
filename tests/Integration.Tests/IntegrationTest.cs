namespace Integration.Tests
{
    using System;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public class IntegrationTest
    {
        protected Func<IMediator> Mediator { get; }

        public IntegrationTest()
        {
            IServiceCollection services = new ServiceCollection();

            IServiceProvider provider = services.BuildServiceProvider();

            Mediator = () => provider.GetService<IMediator>();
        }
    }
}