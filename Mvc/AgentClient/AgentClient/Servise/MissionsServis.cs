using AgentClient.Dto;
using AgentClient.ViewModel;
using System.Text;
using System.Text.Json;

namespace AgentClient.Servise
{
    public class MissionsServis(IHttpClientFactory clientFactory, IServiceProvider serviceProvider) : IMissionsServis
    {
        private IAgentServis agentService => serviceProvider.GetRequiredService<IAgentServis>();
        private ITargetServis targetService => serviceProvider.GetRequiredService<ITargetServis>();

        private readonly string baseUrl = "https://localhost:7220/";

        public async Task<List<MissionDto>?> GetAllMissionsFormServerAsync()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync($"{baseUrl}Missions");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<MissionDto>? missions = JsonSerializer.Deserialize<List<MissionDto>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
                return missions ?? null;
            }
            return null;
        }

        // Creates a list of active tasks.
        public async Task<List<MissionsManagementVM>?> CreateListMissionsManagementVM()
        {
            var allMissions = await GetAllMissionsFormServerAsync();
            var allAgents = await agentService.GetAllAgentsFormServerAsync();
            var allTargets = await targetService.GetAllTargetsFormServerAsync();

            if (allTargets == null || allAgents == null || allMissions == null)
                return null;

            var vM = allMissions.Where(x => x.Status == Dto.MissionStatus.Proposal)
                .Select(x => new MissionsManagementVM
                {
                    MissionId = x.Id,
                    AgentName = allAgents.FirstOrDefault(a => a.Id == x.AgentId)!.Name,
                    AgentLocationX = allAgents.FirstOrDefault(a => a.Id == x.AgentId)!.locationX,
                    AgentLocationY = allAgents.FirstOrDefault(a => a.Id == x.AgentId)!.locationY,
                    TargetName = allTargets.FirstOrDefault(a => a.Id == x.TargetId)!.Name,
                    TargetRole = allTargets.FirstOrDefault(a => a.Id == x.TargetId)!.Role,
                    TargetLocationX = allTargets.FirstOrDefault(a => a.Id == x.TargetId)!.locationX,
                    TargetLocationY = allTargets.FirstOrDefault(a => a.Id == x.TargetId)!.locationY,
                    Distance = x.TimeLeft * 5,
                    TimeToEliminate = x.TimeLeft,
                })
                .ToList();

            return vM;
        }
        public async Task<bool> StatusChangeById(int id, string status)
        {
            var httpClient = clientFactory.CreateClient();
            ResStatusDto dto = new ResStatusDto
            {
                Status = status,
            };
            var httpContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var result = await httpClient.PutAsync($"{baseUrl}Missions/{id}", httpContent);
            return result.IsSuccessStatusCode;
        }
    }
}
