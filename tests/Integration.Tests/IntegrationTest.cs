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

    public class IntegrationTest
    {
        protected Func<IMediator> Mediator { get; }

        public IntegrationTest()
        {
            Container container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.RegisterSingleton<IMediator, Mediator>();

            container.Register(
                () => new ServiceFactory(container.GetInstance),
                Lifestyle.Singleton);

            Assembly[] assemblies = { typeof(IProcessLivesHere).Assembly };

            // handlers
            container.Register(typeof(IRequestHandler<,>), assemblies);

            // notifications
            var notificationHandlerTypes = container.GetTypesToRegister(
                typeof(INotificationHandler<>),
                assemblies,
                new TypesToRegisterOptions
                {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = false,
                });

            // notification handlers
            container.Collection.Register(
                typeof(INotificationHandler<>),
                notificationHandlerTypes);

            // pre-processors
            container.Collection.Register(
                typeof(IRequestPreProcessor<>),
                new Type[] { });

            // post processors
            container.Collection.Register(
                typeof(IRequestPostProcessor<,>),
                new[] { typeof(Sender<,>) });

            // pipeline behaviors
            container.Collection.Register(typeof(IPipelineBehavior<,>), new[]
            {
                typeof(RequestPreProcessorBehavior<,>),
                typeof(RequestPostProcessorBehavior<,>),
                typeof(FeatureBehavior<,>)
            });

            // other services
            container.RegisterSingleton<IStoreDocuments, DocumentStore>();

            container.Register<IStoreTabularData, TableStore>();

            // validation
            container.Collection.Register(typeof(IValidator<>), assemblies);

            container.Verify();

            Mediator = () => container.GetInstance<IMediator>();
        }
    }
}