using AgentRest.Data;
using AgentRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Servise
{
    public class MissionServis(ApplicationDbContext context) : IMissionServis
    {
        public async Task<List<MissionModel>> GetAllMissionsAsync() =>
            await context.Missions.ToListAsync();
    }
}
