using AgentClient.Dto;
using System.Text.Json;

namespace AgentClient.Servise
{
    public class AgentServis(IHttpClientFactory clientFactory) : IAgentServis
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
    }
}
