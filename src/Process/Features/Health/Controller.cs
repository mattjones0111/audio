namespace Process.Features.Health
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/health")]
    public class Controller : ApiController
    {
        [HttpGet("")]
        public async Task<ActionResult> Index(Index.Query query) =>
            await Ok(Mediator.Send(query ?? new Index.Query()));
    }
}
