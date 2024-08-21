using AgentRest.Data;
using AgentRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Servise
{
    public class AgentServis(ApplicationDbContext context) : IAgentServis
    {
        public async Task<List<AgentModel>> GetAllAgentsAsync() =>
           await context.Agents.ToListAsync();
    }
}
