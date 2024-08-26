using AgentClient.Dto;
using AgentClient.ViewModel;

namespace AgentClient.Servise
{
    public interface IMissionsManagementServis
    {
        Task<List<AgentDto>?> GetAllAgentsFormServerAsync();
        Task<List<TargetsDto>?> GetAllTargetsFormServerAsync();
        Task<List<MissionsManagementVM>?> CreateListMissionsManagementVM();
        Task<bool> StatusChangeById(int id, string status);
    }
}
