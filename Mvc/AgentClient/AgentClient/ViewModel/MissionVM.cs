namespace AgentClient.ViewModel
{
    public enum MissionStatus
    {
        Proposal,
        Mitzvah,
        Ended
    }
    public class MissionVM
    {

        public int Id { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public double TimeLeft { get; set; }
        public double TimeRight { get; set; }
        public MissionStatus Status { get; set; }
    }
}
