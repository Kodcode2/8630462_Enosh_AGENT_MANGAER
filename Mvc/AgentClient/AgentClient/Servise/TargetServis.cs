using AgentClient.Dto;
using System.Text.Json;

namespace AgentClient.Servise
{
    public class TargetServis(IHttpClientFactory clientFactory) : ITargetServis
    {
        private readonly string baseUrl = "https://localhost:7220/";
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
    }
}
