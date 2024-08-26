using AgentClient.Dto;
using AgentClient.ViewModel;

namespace AgentClient.Servise
{
    public interface IMissionsServis
    {
        Task<List<MissionDto>?> GetAllMissionsFormServerAsync();
        Task<List<MissionsManagementVM>?> CreateListMissionsManagementVM();
        Task<bool> StatusChangeById(int id, string status);
    }
}
