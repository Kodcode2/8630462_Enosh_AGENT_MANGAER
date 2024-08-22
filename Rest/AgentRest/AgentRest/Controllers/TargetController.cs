using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Servise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("api/target")]
    [ApiController]
    public class TargetController(ITargetServis targetServis) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<TargetModel>>> GetAll([FromBody] TokenDto token) => Ok(await targetServis.GetAllTargetsAsync(token));

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // The request returns a status of 200 because, according to the characterization, the created ID should be returned.
        public async Task<ActionResult<int>> CreateTarget([FromBody] TargetDto model)
        {
            try
            {
                return Ok(await targetServis.CreateTargetAsync(model));
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
                return Ok(await targetServis.CreateLocationAsync(id, location));
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
                return Ok(await targetServis.MovementAsync(id, direction.Direction));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
