namespace Process.Features.Audio.Markers
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/audio/{id}/markers")]
    public class Controller : ApiController
    {
        [HttpPost]
        public Task<ActionResult> Add([FromBody] Add.Command command) =>
            ExecuteAsync(command);

        [HttpDelete("{offset}/{name}")]
        public Task<ActionResult> Delete([FromQuery] Remove.Command command) =>
            ExecuteAsync(command);
    }
}