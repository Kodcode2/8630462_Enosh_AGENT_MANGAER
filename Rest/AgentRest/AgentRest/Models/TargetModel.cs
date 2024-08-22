namespace AgentRest.Models
{
    public enum TargetStatus
    {
        Alive,
        Destroyed
    }
    public class TargetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
        public int locationX { get; set; } = -201;
        public int locationY { get; set; } = -201;
        public TargetStatus Status { get; set; }
    }
}
