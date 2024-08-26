using AgentClient.Dto;

namespace AgentClient.ViewModel
{
    public class TargetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
        public int locationX { get; set; }
        public int locationY { get; set; }
        public TargetStatus Status { get; set; }

    }
}
