namespace Process.Features.Audio
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/audio")]
    public class Controller : ControllerBase
    {
        readonly IMediator mediator;

        public Controller(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("")]
        public async Task<ActionResult> Index() =>
            Ok(await mediator.Send(new Index.Query()));
    }
}