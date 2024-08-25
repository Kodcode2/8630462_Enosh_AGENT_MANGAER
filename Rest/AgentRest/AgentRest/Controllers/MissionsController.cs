using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Servise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController(IMissionServis missionServis) : ControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult<List<MissionModel>>> GetAll([FromBody] TokenDto token) => Ok(await missionServis.GetAllMissionsAsync(token));

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdetStatusMissions(int id, [FromBody] MissionDto mission)
        {
            try
            {
                var activatingTheFunction = await missionServis.TaskUpdateStatus(id, mission);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdetStatusMissions()
        {
            try
            {
                var activatingTheFunction = await missionServis.CreateMissionAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
