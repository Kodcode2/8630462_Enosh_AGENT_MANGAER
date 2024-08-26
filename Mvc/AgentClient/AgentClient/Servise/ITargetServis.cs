using AgentClient.Dto;

namespace AgentClient.Servise
{
    public interface ITargetServis
    {
        Task<List<TargetsDto>?> GetAllTargetsFormServerAsync();
    }
}
