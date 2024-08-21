using AgentRest.Models;

namespace AgentRest.Servise
{
    public interface IMissionServis
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
    }
}