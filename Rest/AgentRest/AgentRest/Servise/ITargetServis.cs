using AgentRest.Models;

namespace AgentRest.Servise
{
    public interface ITargetServis
    {
        Task<List<TargetModel>> GetAllTargetsAsync();
    }
}
