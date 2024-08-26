namespace AgentClient.Dto
{
    public enum TargetStatus
    {
        Alive,
        Destroyed
    }
    public class TargetsDto
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
