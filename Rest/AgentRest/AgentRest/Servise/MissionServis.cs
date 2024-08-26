using AgentRest.Data;
using AgentRest.Dto;
using AgentRest.Models;
using AgentRest.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace AgentRest.Servise
{
    public class MissionServis(ApplicationDbContext context, IServiceProvider serviceProvider) : IMissionServis
    {
        private IAgentServis agentService => serviceProvider.GetRequiredService<IAgentServis>();
        private ITargetServis targetService => serviceProvider.GetRequiredService<ITargetServis>();



        public async Task<List<MissionModel>> GetAllMissionsAsync() =>
        await context.Missions.ToListAsync();

        public async Task<MissionModel> TaskUpdateStatus(int id, MissionDto mission)
        {
            try
            {
                var missionIsExsist = await context.Missions.FirstOrDefaultAsync(x => x.Id == id);

                if (missionIsExsist == null)
                {
                    throw new Exception($"mission with the {id} does not exist");
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
                    // change status of agent.
                    var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Id == missionIsExsist.AgentId);
                    agentIsExsist!.Status = AgentStatus.Operations;

                    // Calculation of time according to Id.
                    missionIsExsist.TimeLeft = await CalculationOfTime(missionIsExsist.AgentId, missionIsExsist.TargetId);
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
                    mission.TimeRight = await CalculationOfTime(mission.AgentId, mission.TargetId);
                }
                mission.TimeLeft -= 0.0005;
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

        // Calculation of time according to Id.
        public async Task<double> CalculationOfTime(int agentId, int targetId)
        {
            var agentIsExsist = await context.Agents.FirstOrDefaultAsync(x => x.Id == agentId);
            var targetIsExsist = await context.Targets.FirstOrDefaultAsync(x => x.Id == targetId);
            if (agentIsExsist == null || targetIsExsist == null)
                return -1;
            return DistanceCalculation
                .CalculationOperationTime(DistanceCalculation
                .CalculateDistance(agentIsExsist.locationX, agentIsExsist.locationY, targetIsExsist.locationX, targetIsExsist.locationY));
        }

    }
}
