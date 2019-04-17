namespace Process.Features.Audio
{
    using System.Threading.Tasks;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/audio")]
    public class Controller : ApiController
    {
        readonly IMediator mediator;

        public Controller(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("")]
        public async Task<ActionResult> Index(Index.Query query) =>
            await Ok(mediator.Send(query ?? new Index.Query()));

        [HttpPost("")]
        public async Task<ActionResult> Create(Create.Command command) =>
            await NoContent(mediator.Send(command));

        [HttpDelete("")]
        public async Task<ActionResult> Delete(Delete.Command command) =>
            await NoContent(mediator.Send(command));
    }
}
