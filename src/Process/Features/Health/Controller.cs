namespace Process.Features.Health
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/health")]
    public class Controller : ControllerBase
    {
        readonly IMediator mediator;

        public Controller(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("")]
        public async Task<ActionResult> Index(Index.Query query) =>
            Ok(await mediator.Send(query ?? new Index.Query()));
    }
}
