using AgentRest.Data;
using AgentRest.Dto;
using AgentRest.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AgentRest.Servise
{
    public class MissionServis(ApplicationDbContext context) : IMissionServis
    {
        public async Task<List<MissionModel>> GetAllMissionsAsync(TokenDto token) =>
        await context.Missions.ToListAsync();

        public async Task<MissionModel> TaskUpdateStatus(int id, MissionDto mission)
        {
            var missionIsExsist = await context.Missions.FirstOrDefaultAsync(x => x.Id == id);

            if (missionIsExsist == null)
            {
                throw new Exception($"Agent with the {id} does not exist");
            }

            missionIsExsist.Status = mission.Status switch
            {
                "proposal" => MissionStatus.Proposal,
                "mitzvah" => MissionStatus.Mitzvah,
                "ended" => MissionStatus.Ended,
                _ => throw new Exception($"Invalid value {mission.Status}")
            };

            await context.SaveChangesAsync();
            return missionIsExsist;
        }

        //public async Task<MissionModel> CreateMission()
        //{

        //}
    }
}
