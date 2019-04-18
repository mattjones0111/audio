namespace Integration.Tests
{
    using System;
    using System.Reflection;
    using FluentValidation;
    using MediatR;
    using MediatR.Pipeline;
    using Process;
    using Process.Adapters.InMemory;
    using Process.Aspects.Notifications;
    using Process.Pipeline;
    using Process.Ports;
    using SimpleInjector;
    using SimpleInjector.Lifestyles;
    using DocumentStore = Adapter.Azure.DocumentStore;

    public class IntegrationTest
    {
        protected Func<IMediator> Mediator { get; }

        public IntegrationTest()
        {
            Container container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.RegisterSingleton<IMediator, Mediator>();

            Assembly[] assemblies = { typeof(IProcessLivesHere).Assembly };

            container.Register(typeof(IRequestHandler<,>), assemblies);

            var notificationHandlerTypes = container.GetTypesToRegister(
                typeof(INotificationHandler<>),
                assemblies,
                new TypesToRegisterOptions
                {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = false,
                });
            container.Collection.Register(typeof(INotificationHandler<>), notificationHandlerTypes);

            container.Collection.Register(typeof(IRequestPreProcessor<>), new Type[] { });
            container.Collection.Register(typeof(IRequestPostProcessor<,>), new[] { typeof(Sender<,>) });

            container.Collection.Register(typeof(IPipelineBehavior<,>), new[]
            {
                typeof(RequestPreProcessorBehavior<,>),
                typeof(RequestPostProcessorBehavior<,>),
                typeof(FeatureBehavior<,>)
            });

            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);

            container.Register<IStoreDocuments>(() =>
                new DocumentStore("UseDevelopmentStorage=true", "documents"));

            container.Register<IStoreTabularData, TableStore>();

            // validation
            container.Collection.Register(typeof(IValidator<>), assemblies);

            container.Verify();

            Mediator = () => container.GetInstance<IMediator>();
        }
    }
}