namespace Process.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.Extensions.DependencyInjection;
    using Pipeline;

    public abstract class ApiController : ControllerBase
    {
        public IMediator Mediator { protected get; set; }

        protected async Task<ActionResult> ExecuteAsync<T>(Query<T> query)
        {
            T result = await Mediator.Send(query);
            return Ok(result);
        }

        protected async Task<ActionResult> ExecuteAsync(Command command)
        {
            CommandResult result = await Mediator.Send(command);

            if (result.HasResponse)
            {
                return Ok(result.Response);
            }

            return NoContent();
        }
    }

    public class MediatorProviderControllerFactoryDecorator : IControllerFactory
    {
        readonly IControllerFactory innerFactory;
        readonly IServiceProvider serviceProvider;

        public MediatorProviderControllerFactoryDecorator(
            IControllerFactory innerFactory,
            IServiceProvider serviceProvider)
        {
            this.innerFactory = innerFactory;
            this.serviceProvider = serviceProvider;
        }

        public object CreateController(ControllerContext context)
        {
            object result = innerFactory.CreateController(context);
            
            ApiController apiController = result as ApiController;
            if (apiController != null)
            {
                apiController.Mediator =
                    serviceProvider.GetRequiredService<IMediator>();
            }

            return result;
        }

        public void ReleaseController(ControllerContext context, object controller)
        {
            innerFactory.ReleaseController(context, controller);
        }
    }
}
