using AgentRest.Models;

namespace AgentRest.Servise
{
    public interface IAgentServis
    {
        Task<List<AgentModel>> GetAllAgentsAsync();
    }
}
