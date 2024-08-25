using AgentRest.Data;
using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AgentRest.Servise
{
    public class AgentServis(ApplicationDbContext context) : IAgentServis
    {
        //private readonly object _lock = new object();
        //private Task ShmuelTask()
        //{
        //    lock (_lock)
        //    {

        //    }
        //}
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

            await CheckingTasks(agentIsExsist);

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

            List<int> notAllowedRange = [-1, 1001];

            Validator<AgentModel>.Of(agentIsExsist)
                .Validate(a => a != null, $"Agent with the {id} does not exist")
                .Validate(a => a!.locationX != -1, $"Location Not booted yet")
                .Validate(a => a!.Status != AgentStatus.Operations, $"The agent is operations")
                .Validate<Dictionary<string, (int, int)>>(d => d.Any(x => x.Key == direction), WalkingCoordinates, $"There is no {direction} walking function")
                .ThrowFirst();

            agentIsExsist!.locationX += WalkingCoordinates.First(x => x.Key == direction).Value.x;
            agentIsExsist!.locationY += WalkingCoordinates.First(x => x.Key == direction).Value.y;

            if (notAllowedRange.Any(na => agentIsExsist.locationX == na || agentIsExsist.locationY == na))  
                throw new Exception("It is not possible to run outside the formation");

            await CheckingTasks(agentIsExsist);

            await context.SaveChangesAsync();
            return agentIsExsist;
        }

        // Test to create a Mission
        private async Task<AgentModel> CheckingTasks(AgentModel agentModel)
        {
            // Running on each target separately to check the distance from the agent
            foreach (var target in await context.Targets.ToListAsync())
            {
                var distanceCalculation = DistanceCalculation.CalculateDistance(agentModel.locationX, agentModel.locationY, target.locationX, target.locationY);
                if (distanceCalculation < 200)
                {
                    // If the task already exists, continue to the next target
                    if (await context.Missions.FirstOrDefaultAsync(x => x.AgentId == agentModel.Id && x.TargetId == target.Id) != null)
                        continue;

                    // Create a new task
                    MissionModel newMission = new()
                    {
                        AgentId = agentModel.Id,
                        TargetId = target.Id,
                        Status = MissionStatus.Proposal,
                    };
                    await context.Missions.AddAsync(newMission);
                }

                // There is an existing task where there is not enough distance to perform a deletion.
                else
                {
                    var existingTask = await context.Missions.FirstOrDefaultAsync(x => x.AgentId == agentModel.Id && x.TargetId == target.Id);
                    if (existingTask != null)
                        context.Missions.Remove(existingTask);
                }
            }
            // The return of the object (it is not really necessary but probably for future use).
            return agentModel;
        }


        public async Task<AgentModel?> MoveToTargetAsync(int id, string direction)
        {
            if (direction == string.Empty) return null;

            var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Id == id);

            List<int> notAllowedRange = [-1, 1001];

            Validator<AgentModel>.Of(agentIsExsist)
                .Validate(a => a != null, $"Agent with the {id} does not exist")
                .Validate<Dictionary<string, (int, int)>>(d => d.Any(x => x.Key == direction), WalkingCoordinates, $"There is no {direction} walking function")
                .ThrowFirst();

            agentIsExsist!.locationX += WalkingCoordinates.First(x => x.Key == direction).Value.x;
            agentIsExsist!.locationY += WalkingCoordinates.First(x => x.Key == direction).Value.y;

            if (notAllowedRange.Any(na => agentIsExsist.locationX == na || agentIsExsist.locationY == na))
                throw new Exception("It is not possible to run outside the formation");

            await CheckingTasks(agentIsExsist);

            await context.SaveChangesAsync();
            return agentIsExsist;
        }
    }
}
