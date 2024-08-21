using AgentRest.Models;
using AgentRest.Servise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetController(ITargetServis targetServis) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<TargetModel>>> GetAll() => Ok(await targetServis.GetAllTargetsAsync());


    }
}
