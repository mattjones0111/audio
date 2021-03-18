namespace Process.Features.Audio.Categories
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/audio/{id}/categories")]
    public class Controller : ApiController
    {
        [HttpPost("{category}")]
        public Task<ActionResult> Add([FromQuery] Add.Command command) =>
            ExecuteAsync(command);
    }
}
