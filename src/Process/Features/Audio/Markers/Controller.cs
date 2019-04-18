namespace Process.Features.Audio.Markers
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/audio/{id}/markers")]
    public class Controller : ApiController
    {
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Add.Command command) =>
            await NoContent(Mediator.Send(command));

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] Delete.Command command) =>
            await NoContent(Mediator.Send(command));
    }
}