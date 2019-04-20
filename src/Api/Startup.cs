namespace Api
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using FluentValidation;
    using MediatR;
    using MediatR.Pipeline;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.ViewComponents;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Middleware;
    using Process;
    using Process.Adapters.InMemory;
    using Process.Aspects.Notifications;
    using Process.Infrastructure;
    using Process.Pipeline;
    using Process.Ports;
    using SimpleInjector;
    using SimpleInjector.Integration.AspNetCore.Mvc;
    using SimpleInjector.Lifestyles;
    using Swashbuckle.AspNetCore.Swagger;
    using DocumentStore = Adapter.Azure.DocumentStore;

    public class Startup
    {
        readonly Container container = new Container();
    
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Audio", Version = "v1" });
                c.SchemaRegistryOptions.SchemaIdSelector = type => type.FullName;
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            IntegrateSimpleInjector(services);
        }

        void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddHttpContextAccessor();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));
            
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }
        
        void InitializeContainer(IApplicationBuilder app)
        {
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            container.AutoCrossWireAspNetComponents(app);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Audio Api v1");
            });

            InitializeContainer(app);

            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(
                () => new ServiceFactory(container.GetInstance),
                Lifestyle.Singleton);

            Assembly[] assemblies = { typeof(IProcessLivesHere).Assembly };

            // handlers
            container.Register(typeof(IRequestHandler<,>), assemblies);

            // notifications
            IEnumerable<Type> notificationHandlerTypes = container
                .GetTypesToRegister(
                    typeof(INotificationHandler<>),
                    assemblies,
                    new TypesToRegisterOptions
                    {
                        IncludeGenericTypeDefinitions = true,
                        IncludeComposites = false,
                    });

            container.Collection.Register(
                typeof(INotificationHandler<>),
                notificationHandlerTypes);

            // pre and post processors
            container.Collection.Register(
                typeof(IRequestPreProcessor<>),
                new Type[] { });

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
            container.Register<IStoreDocuments>(() =>
                new DocumentStore("UseDevelopmentStorage=true", "documents"));

            container.Register<IStoreTabularData, TableStore>();

            // validation
            container.Collection.Register(typeof(IValidator<>), assemblies);

            container.RegisterInitializer<ApiController>(controller =>
            {
                controller.Mediator = container.GetInstance<IMediator>();
            });

            container.Verify();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseExceptionHandlingMiddleware();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
