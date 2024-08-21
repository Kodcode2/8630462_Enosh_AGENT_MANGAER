using AgentRest.Models;
using AgentRest.Servise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController(IAgentServis agentServis) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AgentModel>>> GetAll() => Ok(await agentServis.GetAllAgentsAsync());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentModel>> CreateUser([FromBody] AgentModel model)
        {
            try
            {
                return Created("", await agentServis.CreateAgentAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
