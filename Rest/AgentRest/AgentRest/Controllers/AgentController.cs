using AgentRest.Dto;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // The request returns a status of 200 because, according to the characterization, the created ID should be returned.
        public async Task<ActionResult<int>> CreateAgent([FromBody] AgentDto model)
        {
            try
            {
                return Ok(await agentServis.CreateAgentAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentModel>> StartPin(int id, [FromBody] LocationDto location)
        {
            try
            {
                return Ok(await agentServis.CreateLocationAsync(id , location));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentModel>> Walking(int id, [FromBody] DirectionDto direction)
        {
            try
            {
                return Ok(await agentServis.MovementAsync(id, direction.Direction));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
