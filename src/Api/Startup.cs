namespace Api
{
    using System;
    using System.Reflection;
    using Adapter.Azure;
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
    using Process;
    using Process.Aspects.Notifications;
    using Process.Pipeline;
    using SimpleInjector;
    using SimpleInjector.Integration.AspNetCore.Mvc;
    using SimpleInjector.Lifestyles;

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
            InitializeContainer(app);

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

            container.Register(() =>
                new DocumentStore("UseDevelopmentStorage=true", "documents"));

            // validation
            container.Collection.Register(typeof(IValidator<>), assemblies);

            container.Verify();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
