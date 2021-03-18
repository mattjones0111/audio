namespace Process.Features.Audio
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/audio")]
    public class Controller : ApiController
    {
        [HttpGet("")]
        public Task<ActionResult> Index(Index.Query query) =>
            ExecuteAsync(query ?? new Index.Query());

        [HttpPost("")]
        public Task<ActionResult> Create(Create.Command command) =>
            ExecuteAsync(command);

        [HttpDelete("")]
        public Task<ActionResult> Delete(Delete.Command command) =>
            ExecuteAsync(command);
    }
}
