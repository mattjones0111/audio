namespace Process.Features.Health
{
    using System.Threading.Tasks;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/health")]
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
    }
}
