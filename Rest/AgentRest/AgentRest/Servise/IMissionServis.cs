using AgentRest.Dto;
using AgentRest.Models;

namespace AgentRest.Servise
{
    public interface IMissionServis
    {
        Task<List<MissionModel>> GetAllMissionsAsync(TokenDto token);
        Task<MissionModel> TaskUpdateStatus(int id, MissionDto mission);
    }
}