using AgentRest.Data;
using AgentRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Servise
{
    public class TargetServis(ApplicationDbContext context) : ITargetServis
    {
        public async Task<List<TargetModel>> GetAllTargetsAsync() =>
            await context.Targets.ToListAsync();
    }
}
