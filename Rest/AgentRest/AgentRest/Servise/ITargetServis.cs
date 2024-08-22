using AgentRest.Dto;
using AgentRest.Models;

namespace AgentRest.Servise
{
    public interface ITargetServis
    {
        Task<List<TargetModel>> GetAllTargetsAsync(TokenDto token);
        Task<ResIdDto?> CreateTargetAsync(TargetDto target);
        Task<TargetModel?> CreateLocationAsync(int id, LocationDto location);
        Task<TargetModel?> MovementAsync(int id, string direction);
    }
}
