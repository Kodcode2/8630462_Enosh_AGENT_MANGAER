using Microsoft.SqlServer.Server;

namespace AgentRest.Models
{

    public enum AgentStatus
    {
        Dormant,
        Operations
    }
    public class AgentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int locationX { get; set; } = -1;
        public int locationY { get; set; } = -1;
        public List<MissionModel> Missions { get; set; } = [];
        public AgentStatus Status { get; set; }

    }
}
