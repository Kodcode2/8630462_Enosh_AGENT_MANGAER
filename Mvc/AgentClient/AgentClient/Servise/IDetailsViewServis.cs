using AgentClient.Dto;

namespace AgentClient.Servise
{
    public interface IDetailsViewServis
    {
        Task<List<MissionDto>?> GetAllMissionsFormServerAsync();
    }
}
