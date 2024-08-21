using AgentRest.Models;
using AgentRest.Servise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController(IAgentServis agentServis) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<TargetModel>>> GetAll() => Ok(await agentServis.GetAllAgentsAsync());
    }
}
