using AgentClient.Dto;
using AgentClient.ViewModel;

namespace AgentClient.Servise
{
    public interface IDashboardsServis
    {
        Task<GeneralInformationVM> SetGeneralInformationVM();
        Task<List<AgentDto>?> GetAllAgentsFormServerAsync();
        Task<List<TargetsDto>?> GetAllTargetsFormServerAsync();
        Task<List<AgentVM>> AgentDetails();
        Task<List<TargetVM>> TargetDetails();
    }
}
