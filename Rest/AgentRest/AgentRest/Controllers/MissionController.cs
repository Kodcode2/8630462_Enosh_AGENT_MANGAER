using AgentRest.Models;
using AgentRest.Servise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController(IMissionServis missionServis) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<MissionModel>>> GetAll() => Ok(await missionServis.GetAllMissionsAsync());
    }
}
