namespace AgentClient.Dto
{
    public enum AgentStatus
    {
        Dormant,
        Operations
    }
    public class AgentDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int locationX { get; set; }
        public int locationY { get; set; }
        public List<MissionDto> Missions { get; set; } = [];
        public AgentStatus Status { get; set; }
    }
}
