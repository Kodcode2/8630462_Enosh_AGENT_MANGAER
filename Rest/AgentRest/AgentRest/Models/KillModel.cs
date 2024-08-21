namespace AgentRest.Models
{
    public class KillModel
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public AgentModel Agent { get; set; }
        public TargetModel Target { get; set; }
        public DateTime ExecutionTime { get; set; }
    }
}
