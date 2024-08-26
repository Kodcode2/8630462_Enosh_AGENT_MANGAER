using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Servise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController(ITargetServis targetServis) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<TargetModel>>> GetAll() => Ok(await targetServis.GetAllTargetsAsync());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // The request returns a status of 200 because, according to the characterization, the created ID should be returned.
        public async Task<ActionResult<ResIdDto>> CreateTarget([FromBody] TargetDto model)
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> StartPin(int id, [FromBody] LocationDto location)
        {
            try
            {
                var targetModel = await targetServis.CreateLocationAsync(id, location);
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
                var targetModel = await targetServis.MovementAsync(id, direction.direction);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
