using AgentClient.Dto;
using AgentClient.ViewModel;
using System.Reflection;
using System.Text.Json;

namespace AgentClient.Servise
{
    public class DashboardsServis(IHttpClientFactory clientFactory, IDetailsViewServis detailsViewServis) : IDashboardsServis
    {

        private readonly string baseUrl = "https://localhost:7220/";
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
                AmountAgentsActivity = allAgents.Where(x => x.Status == AgentStatus.Operations).Count(),

                AmountTargets = allTargets.Count(),
                AmountTargetsEliminated = allTargets.Where(x => x.Status == TargetStatus.Destroyed).Count(),

                AmountMissions = allMissions.Count(),
                AmountMissionsActivity = allMissions.Where(x => x.Status == Dto.MissionStatus.Mitzvah).Count(),

                RelationAgentsTargets = allAgents.Count() / allTargets.Count(),
                RelationAgentsTargetsTeamable = allAgents.Where(x => x.Status == AgentStatus.Dormant).Count() 
                / allTargets.Where(x => x.Status == TargetStatus.Alive).Count(),
            };
            return generalInformationVM;
        }
        
        public async Task<List<AgentVM>> AgentDetails()
        {
            var allMissions = await detailsViewServis.GetAllMissionsFormServerAsync();
            var allAgents = await GetAllAgentsFormServerAsync();
            var allTargets = await GetAllTargetsFormServerAsync();

            if (allMissions == null || allAgents == null)
                return [];

            List<AgentVM> ListVM = allAgents.Select(x => new AgentVM
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image,
                locationX = x.locationX,
                locationY = x.locationY,
                MissionId = allMissions.Any(m => m.AgentId == x.Id && m.Status == Dto.MissionStatus.Mitzvah) ? allMissions.FirstOrDefault(m => m.AgentId == x.Id)!.Id : -1,
                Status = x.Status,
                TimeLeft = allMissions.Any(m => m.AgentId == x.Id && m.Status == Dto.MissionStatus.Mitzvah) ? allMissions.FirstOrDefault(m => m.AgentId == x.Id)!.TimeLeft : -1,
                AmountEliminations = allMissions.Where(x => x.Status == Dto.MissionStatus.Ended).Count(),
            }).ToList();


            return ListVM;
        }

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
