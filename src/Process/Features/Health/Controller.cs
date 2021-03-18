namespace Process.Features.Health
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/health")]
    public class Controller : ApiController
    {
        [HttpGet("")]
        public Task<ActionResult> Index(Index.Query query) =>
            ExecuteAsync(query ?? new Index.Query());
    }
}
