using AgentClient.Dto;
using AgentClient.ViewModel;
using System.Text;
using System.Text.Json;

namespace AgentClient.Servise
{
    public class MissionsManagementServis(IHttpClientFactory clientFactory, IServiceProvider serviceProvider) : IMissionsManagementServis
    {
        private IDetailsViewServis detailsViewServis => serviceProvider.GetRequiredService<IDetailsViewServis>();

        private readonly string baseUrl = "https://localhost:7220/";

        // Returns all Agents
        public async Task<List<AgentDto>?> GetAllAgentsFormServerAsync()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync($"{baseUrl}Agents");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<AgentDto>? agents = JsonSerializer.Deserialize<List<AgentDto>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
                return agents ?? null;
            }
            return null;
        }

        // Returns all Targets
        public async Task<List<TargetsDto>?> GetAllTargetsFormServerAsync()
        {
            var httpClient = clientFactory.CreateClient();
            var result = await httpClient.GetAsync($"{baseUrl}Targets");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<TargetsDto>? targets = JsonSerializer.Deserialize<List<TargetsDto>>(
                    content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
                return targets ?? null;
            }
            return null;
        }

        // Creates a list of active Missions.
        public async Task<List<MissionsManagementVM>?> CreateListMissionsManagementVM()
        {
            var allMissions = await detailsViewServis.GetAllMissionsFormServerAsync();
            var allAgents = await GetAllAgentsFormServerAsync();
            var allTargets = await GetAllTargetsFormServerAsync();

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

        // Changes the status and returns bool if changed
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
