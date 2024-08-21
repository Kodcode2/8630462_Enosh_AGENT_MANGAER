using AgentRest.Data;
using AgentRest.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AgentRest.Servise
{
    public class AgentServis(IHttpClientFactory clientFactory, ApplicationDbContext context) : IAgentServis
    {
        private readonly string baseUrl = "https://localhost:7244/api/User";

        public async Task<List<AgentModel>> GetAllAgentsAsync() =>
           await context.Agents.ToListAsync();

        public async Task<AgentModel?> CreateAgentAsync(AgentModel agent)
        {
            var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Name == agent.Name);

            if (agentIsExsist != null)
            {
                throw new Exception("agent with this name already exists");
            }
            await context.Agents.AddAsync(agent);
            await context.SaveChangesAsync();
            return agent;
        }
        



    }
}
