using AgentRest.Dto;
using AgentRest.Models;

namespace AgentRest.Servise
{
    public interface IAgentServis
    {
        Task<List<AgentModel>> GetAllAgentsAsync();
        Task<ResIdDto?> CreateAgentAsync(AgentDto agent);
        Task<AgentModel?> CreateLocationAsync(int id, LocationDto location);
        Task<AgentModel?> MovementAsync(int id, string direction);
    }
}
