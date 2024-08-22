using AgentRest.Data;
using AgentRest.Dto;
using AgentRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentRest.Servise
{
    public class TargetServis(ApplicationDbContext context) : ITargetServis
    {
        public async Task<List<TargetModel>> GetAllTargetsAsync(TokenDto token) =>
            await context.Targets.ToListAsync();

        public async Task<int?> CreateTargetAsync(TargetDto target)
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
                Image = target.Photo_url
            };
            await context.Targets.AddAsync(targetModel);
            await context.SaveChangesAsync();

            return targetModel.Id;
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

            if (targetIsExsist == null)
                throw new Exception($"Targets with the {id} does not exist");

            if (targetIsExsist.locationX == -1)
                throw new Exception($"Location Not booted yet");

            if (!WalkingCoordinates.Any(x => x.Key == direction))
                throw new Exception($"There is no {direction} walking function");

            targetIsExsist.locationX += WalkingCoordinates.First(x => x.Key == direction).Value.x;
            targetIsExsist.locationY += WalkingCoordinates.First(x => x.Key == direction).Value.y;

            if (targetIsExsist.locationX == -1 || targetIsExsist.locationX == 1001 || targetIsExsist.locationY == -1 || targetIsExsist.locationY == 1001)
                throw new Exception("It is not possible to run outside the formation");

            await context.SaveChangesAsync();

            return targetIsExsist;
        }
    }
}
