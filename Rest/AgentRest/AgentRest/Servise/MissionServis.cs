using AgentRest.Data;
using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace AgentRest.Servise
{
    public class MissionServis(ApplicationDbContext context, IServiceProvider serviceProvider) : IMissionServis
    {
        private IAgentServis agentService => serviceProvider.GetRequiredService<IAgentServis>();
        private ITargetServis targetService => serviceProvider.GetRequiredService<ITargetServis>();



        public async Task<List<MissionModel>> GetAllMissionsAsync(TokenDto token) =>
        await context.Missions.ToListAsync();

        public async Task<MissionModel> TaskUpdateStatus(int id, MissionDto mission)
        {
            try
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
                if (missionIsExsist.Status == MissionStatus.Mitzvah)
                {
                    var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Id == missionIsExsist.AgentId);
                    agentIsExsist!.Status = AgentStatus.Operations;
                    // ------------------------------------------------- add -------------------------------------
                }
                await context.SaveChangesAsync();
                return missionIsExsist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<int> CreateMissionAsync()
        {
            // Running on all existing tasks.
            foreach (MissionModel mission in await context.Missions
                .Where(x => x.Status == MissionStatus.Mitzvah)
                .ToListAsync())
            {
                // Returning a list of locations
                var locationByID = await LocationByID(mission.AgentId, mission.TargetId);

                // Agents walking towards the tasks
                var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Id == mission.AgentId);
                string directionForAgents = DistanceCalculation.WhichDirection(locationByID[0], locationByID[1], locationByID[2], locationByID[3]);
                await agentService.MoveToTargetAsync(agentIsExsist!.Id, directionForAgents);

                // Checking whether the Agent and the Target are in the same place.
                // If so, update the status.
                if (locationByID[0] == locationByID[2] && locationByID[1] == locationByID[3])
                {
                    var targetIsExsist = await context.Targets.FirstOrDefaultAsync(x => x.Id == mission.TargetId);
                    agentIsExsist!.Status = AgentStatus.Dormant;
                    targetIsExsist!.Status = TargetStatus.Destroyed;
                    mission.Status = MissionStatus.Ended;
                }
                await context.SaveChangesAsync();
                // Remaining time update
                //mission.TimeLeft = DistanceCalculation.CalculationOperationTime(calculateDistance);
            }
            return 1;
        }

        // Search by ID Agents end Targets, and return the location of both
        private async Task<List<int>> LocationByID(int agentId, int targetId)
        {
            var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Id == agentId);
            var targetIsExsist = await context.Targets.FirstOrDefaultAsync(x => x.Id == targetId);
            if (agentIsExsist == null || targetIsExsist == null)
                return [0, 0, 0, 0];
            return [agentIsExsist.locationX, agentIsExsist.locationY, targetIsExsist.locationX, targetIsExsist.locationY];
        }


        // Stopwatch function
        private async Task MakingPizza(Dictionary<string, int> dic)
        {
            CancellationTokenSource cts = new();
            foreach (var oneDic in dic)
            {
                int tamp = oneDic.Value;
                using PeriodicTimer periodicTimer = new(TimeSpan.FromSeconds(1));
                while (await periodicTimer.WaitForNextTickAsync(cts.Token) && tamp >= 0)
                {
                    Console.WriteLine($"making {oneDic.Key}: {tamp}");
                    tamp--;
                }
                Console.WriteLine($"finished {oneDic.Key}");
            }
        }


    }
}
