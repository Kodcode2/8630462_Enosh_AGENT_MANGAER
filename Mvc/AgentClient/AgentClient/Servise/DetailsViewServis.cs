using AgentClient.Dto;
using System.Text.Json;

namespace AgentClient.Servise
{
    public class DetailsViewServis(IHttpClientFactory clientFactory) : IDetailsViewServis
    {
        private readonly string baseUrl = "https://localhost:7220/";

        // Returns all Missions
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
    }
}
