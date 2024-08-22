using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Servise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController(IAgentServis agentServis) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AgentModel>>> GetAll() => Ok(await agentServis.GetAllAgentsAsync());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // The request returns a status of 200 because, according to the characterization, the created ID should be returned.
        public async Task<ActionResult<ResIdDto>> CreateAgent([FromBody] AgentDto model)
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
        // PUT /agents/id/pin
        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Pin(int id, [FromBody] LocationDto location)
        {
            try
            {
                var targetModel = await agentServis.CreateLocationAsync(id, location);
                if (targetModel == null)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Walking(int id, [FromBody] DirectionDto direction)
        {
            try
            {
                var targetModel = await agentServis.MovementAsync(id, direction.direction);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
