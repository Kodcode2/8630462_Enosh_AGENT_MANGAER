using AgentRest.Data;
using AgentRest.Dto;
using AgentRest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Text.Json;

namespace AgentRest.Servise
{
    public class AgentServis(ApplicationDbContext context) : IAgentServis
    {

        public async Task<List<AgentModel>> GetAllAgentsAsync() =>
           await context.Agents.ToListAsync();

        public async Task<ResIdDto?> CreateAgentAsync(AgentDto agent)
        {
            var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Name == agent.Nickname);

            if (agentIsExsist != null)
            {
                throw new Exception("agent with this name already exists");
            }
            AgentModel agentModel = new()
            {
                Name = agent.Nickname,
                Image = agent.PhotoUrl,
                Status = AgentStatus.Dormant
            };
            await context.Agents.AddAsync(agentModel);
            await context.SaveChangesAsync();

            return new () { Id = agentModel.Id };
        }

        // Change of location according to id
        public async Task<AgentModel?> CreateLocationAsync(int id, LocationDto location)
        {
            var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Id == id);

            if (agentIsExsist == null)
            {
                throw new Exception($"Agent with the {id} does not exist");
            }

            if (location.x < 0 || location.y < 0)
                throw new Exception("illegal place");

            agentIsExsist.locationX = location.x;
            agentIsExsist.locationY = location.y;

            await context.SaveChangesAsync();
            return agentIsExsist;
        }

        private readonly Dictionary<string, (int x, int y)> WalkingCoordinates = new ()
        {
            {"nw", (-1,1) },
            {"n", (0,1) },
            {"ne", (1,1) },
            {"w", (-1,0) },
            {"e", (1,0) },
            {"sw", (-1,-1) },
            {"s", (0,-1) },
            {"se", (1,-1) }

        };
        
        public async Task<AgentModel?> MovementAsync(int id, string direction)
        {
            var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Id == id);

            if (agentIsExsist == null)
                throw new Exception($"Agent with the {id} does not exist");

            if (agentIsExsist.locationX == -1)
                throw new Exception($"Location Not booted yet");

            if (!WalkingCoordinates.Any(x => x.Key == direction))
                throw new Exception($"There is no {direction} walking function");

            agentIsExsist.locationX += WalkingCoordinates.First(x => x.Key == direction).Value.x;
            agentIsExsist.locationY += WalkingCoordinates.First(x => x.Key == direction).Value.y;

            if (agentIsExsist.locationX == -1 || agentIsExsist.locationX == 1001 || agentIsExsist.locationY == -1 || agentIsExsist.locationY == 1001)
                throw new Exception("It is not possible to run outside the formation");

            await context.SaveChangesAsync();
            return agentIsExsist;
        }
    }
}
