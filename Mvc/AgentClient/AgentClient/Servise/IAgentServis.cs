using AgentClient.Dto;

namespace AgentClient.Servise
{
    public interface IAgentServis
    {
        Task<List<AgentDto>?> GetAllAgentsFormServerAsync();
    }
}
