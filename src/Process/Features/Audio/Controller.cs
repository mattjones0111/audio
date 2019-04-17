namespace Process.Features.Audio
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/audio")]
    public class Controller : ApiController
    {
        [HttpGet("")]
        public async Task<ActionResult> Index(Index.Query query) =>
            await Ok(Mediator.Send(query ?? new Index.Query()));

        [HttpPost("")]
        public async Task<ActionResult> Create(Create.Command command) =>
            await NoContent(Mediator.Send(command));

        [HttpDelete("")]
        public async Task<ActionResult> Delete(Delete.Command command) =>
            await NoContent(Mediator.Send(command));
    }
}
