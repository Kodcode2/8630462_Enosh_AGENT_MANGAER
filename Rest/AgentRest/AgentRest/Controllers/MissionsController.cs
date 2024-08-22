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
        public async Task<ActionResult<AgentModel>> UpdetStatusMissions(int id, [FromBody] MissionDto mission)
        {
            try
            {
                return Ok(await missionServis.TaskUpdateStatus(id, mission));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
