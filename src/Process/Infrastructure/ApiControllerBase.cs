namespace Process.Infrastructure
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public abstract class ApiController : ControllerBase
    {
        public IMediator Mediator { protected get; set; }

        protected async Task<ActionResult> Ok<T>(Task<T> contentTask) =>
            Ok(await contentTask);

        protected async Task<ActionResult> NoContent(params Task[] tasks)
        {
            await Task.WhenAll(tasks);

            return base.NoContent();
        }
    }
}
