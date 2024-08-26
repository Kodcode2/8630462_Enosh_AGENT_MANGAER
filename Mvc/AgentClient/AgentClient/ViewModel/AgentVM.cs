using AgentClient.Dto;

namespace AgentClient.ViewModel
{
    public class AgentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int locationX { get; set; }
        public int locationY { get; set; }
        public int MissionId { get; set; }
        public AgentStatus Status { get; set; }
        public double TimeLeft { get; set; }
        public int AmountEliminations { get; set; }
    }
}
