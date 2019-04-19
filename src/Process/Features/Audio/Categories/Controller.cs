namespace Process.Features.Audio.Categories
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/audio/{id}/categories")]
    public class Controller : ApiController
    {
        [HttpPost("{category}")]
        public async Task<ActionResult> Add([FromQuery] Add.Command command) =>
            await NoContent(Mediator.Send(command));
    }
}
