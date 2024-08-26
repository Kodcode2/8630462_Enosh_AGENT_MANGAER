namespace AgentClient.Dto
{
    public enum MissionStatus
    {
        Proposal,
        Mitzvah,
        Ended
    }
    public class MissionDto
    {

        public int Id { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public double TimeLeft { get; set; }
        public double TimeRight { get; set; }

        public MissionStatus Status { get; set; }

    }
}
