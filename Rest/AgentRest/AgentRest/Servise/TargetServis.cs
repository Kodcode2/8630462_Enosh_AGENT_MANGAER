using AgentRest.Data;
using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Utils;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Servise
{
    public class TargetServis(ApplicationDbContext context) : ITargetServis
    {
        public async Task<List<TargetModel>> GetAllTargetsAsync(TokenDto token) =>
            await context.Targets.ToListAsync();

        public async Task<ResIdDto?> CreateTargetAsync(TargetDto target)
        {
            var targetIsExsist = await context.Targets.FirstOrDefaultAsync(x => x.Name == target.Name);

            if (targetIsExsist != null)
            {
                throw new Exception("Targets with this name already exists");
            }
            TargetModel targetModel = new()
            {
                Name = target.Name,
                Role = target.Position,
                Image = target.PhotoUrl
            };
            await context.Targets.AddAsync(targetModel);
            await context.SaveChangesAsync();

            return new() { Id = targetModel.Id };
        }

        // Change of location according to id
        public async Task<TargetModel?> CreateLocationAsync(int id, LocationDto location)
        {
            var targetIsExsist = await context.Targets.FirstOrDefaultAsync(x => x.Id == id);

            if (targetIsExsist == null)
                throw new Exception($"Targets with the {id} does not exist");

            if (location.x < 0 || location.y < 0)
                throw new Exception("illegal place");

            targetIsExsist.locationX = location.x;
            targetIsExsist.locationY = location.y;

            await CheckingTasks(targetIsExsist);

            await context.SaveChangesAsync();

            return targetIsExsist;
        }

        private readonly Dictionary<string, (int x, int y)> WalkingCoordinates = new()
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

        public async Task<TargetModel?> MovementAsync(int id, string direction)
        {
            var targetIsExsist = await context.Targets.FirstOrDefaultAsync(x => x.Id == id);

            Validator<TargetModel>.Of(targetIsExsist)
                .Validate(a => a != null, $"Targets with the {id} does not exist")
                .Validate(a => a!.locationX != -1, $"Location Not booted yet")
                .Validate<Dictionary<string, (int, int)>>(d => d.Any(x => x.Key == direction), WalkingCoordinates, $"There is no {direction} walking function")
                .ThrowFirst();

            targetIsExsist!.locationX += WalkingCoordinates.First(x => x.Key == direction).Value.x;
            targetIsExsist!.locationY += WalkingCoordinates.First(x => x.Key == direction).Value.y;

            if (targetIsExsist.locationX == -1 || targetIsExsist.locationX == 1001 || targetIsExsist.locationY == -1 || targetIsExsist.locationY == 1001)
                throw new Exception("It is not possible to run outside the formation");

            await CheckingTasks(targetIsExsist);

            await context.SaveChangesAsync();

            return targetIsExsist;
        }

        // Test to create a Mission
        private async Task<TargetModel> CheckingTasks(TargetModel targetModel)
        {
            // Running on each agent separately to check the distance from the target
            foreach (var agent in await context.Agents.ToListAsync())
            {
                var distanceCalculation = DistanceCalculation.CalculateDistance(agent.locationX, agent.locationY, targetModel.locationX, targetModel.locationY);
                if (distanceCalculation < 200)
                {
                    // If the task already exists, continue to the next agent
                    if (await context.Missions.FirstOrDefaultAsync(x => x.AgentId == agent.Id && x.TargetId == targetModel.Id) != null)
                        continue;

                    // Create a new task
                    MissionModel newMission = new()
                    {
                        AgentId = agent.Id,
                        TargetId = targetModel.Id,
                        Status = MissionStatus.Proposal,
                    };
                    await context.Missions.AddAsync(newMission);
                }

                // There is an existing task where there is not enough distance to perform a deletion.
                else
                {
                    var existingTask = await context.Missions.FirstOrDefaultAsync(x => x.AgentId == agent.Id && x.TargetId == targetModel.Id);
                    if (existingTask != null)
                        context.Missions.Remove(existingTask);
                }
            }
            // The return of the object (it is not really necessary but probably for future use).
            return targetModel;
        }
    }
}
