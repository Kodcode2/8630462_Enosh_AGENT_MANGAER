using AgentClient.Dto;
using AgentClient.ViewModel;
using System.Reflection;
using System.Text.Json;

namespace AgentClient.Servise
{
    public class DashboardsServis(IHttpClientFactory clientFactory, IDetailsViewServis detailsViewServis) : IDashboardsServis
    {

        private readonly string baseUrl = "https://localhost:7220/";

        // Returns all agents
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

        // Returns all targets
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

        // creation GeneralInformationVM
        public async Task<GeneralInformationVM> SetGeneralInformationVM()
        {
            var allMissions = await detailsViewServis.GetAllMissionsFormServerAsync();
            var allAgents = await GetAllAgentsFormServerAsync();
            var allTargets = await GetAllTargetsFormServerAsync();

            GeneralInformationVM generalInformationVM = new GeneralInformationVM()
            {
                AmountAgents = allAgents.Count(),

                // Returns the amount of Operations agents
                AmountAgentsActivity = allAgents.Where(x => x.Status == AgentStatus.Operations).Count(),

                AmountTargets = allTargets.Count(),
                // Returns the amount of Destroyed Targets
                AmountTargetsEliminated = allTargets.Where(x => x.Status == TargetStatus.Destroyed).Count(),

                AmountMissions = allMissions.Count(),
                // Returns the amount of Mitzvah Missions
                AmountMissionsActivity = allMissions.Where(x => x.Status == Dto.MissionStatus.Mitzvah).Count(),

                // Restores the relationship between agents and targets
                RelationAgentsTargets = allAgents.Count() / allTargets.Count(),

                // Returns the ratio of dormant agents to living targets
                RelationAgentsTargetsTeamable = allAgents.Where(x => x.Status == AgentStatus.Dormant).Count() 
                / allTargets.Where(x => x.Status == TargetStatus.Alive).Count(),
            };
            return generalInformationVM;
        }

        // creator list of AgentVM
        public async Task<List<AgentVM>> AgentDetails()
        {
            var allMissions = await detailsViewServis.GetAllMissionsFormServerAsync();
            var allAgents = await GetAllAgentsFormServerAsync();

            if (allMissions == null || allAgents == null)
                return [];

            List<AgentVM> ListVM = allAgents.Select(x => new AgentVM
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image,
                locationX = x.locationX,
                locationY = x.locationY,
                // If an existing Mission gets the ID and if not gets -1
                MissionId = allMissions.Any(m => m.AgentId == x.Id && m.Status == Dto.MissionStatus.Mitzvah) ? allMissions.FirstOrDefault(m => m.AgentId == x.Id)!.Id : -1,
                Status = x.Status,
                // If an existing Mission gets the TimeLeft and if not gets -1
                TimeLeft = allMissions.Any(m => m.AgentId == x.Id && m.Status == Dto.MissionStatus.Mitzvah) ? allMissions.FirstOrDefault(m => m.AgentId == x.Id)!.TimeLeft : -1,
                AmountEliminations = allMissions.Where(x => x.Status == Dto.MissionStatus.Ended).Count(),
            }).ToList();

            return ListVM;
        }

        // creator list of TargetVM
        public async Task<List<TargetVM>> TargetDetails()
        {
            
            var allTargets = await GetAllTargetsFormServerAsync();

            if (allTargets == null)
                return [];

            List<TargetVM> ListVM = allTargets.Select(x => new TargetVM
            {
                Id = x.Id,
                Name = x.Name,
                Role = x.Role,
                Image = x.Image,
                locationX = x.locationX,
                locationY = x.locationY,
                Status = x.Status,
            }).ToList();


            return ListVM;
        }
    }
}
