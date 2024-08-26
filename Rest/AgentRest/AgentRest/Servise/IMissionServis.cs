using AgentRest.Dto;
using AgentRest.Models;

namespace AgentRest.Servise
{
    public interface IMissionServis
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
        Task<MissionModel> TaskUpdateStatus(int id, MissionDto mission);
        Task<int> CreateMissionAsync();
        Task<double> CalculationOfTime(int agentId, int targetId);
    }
}