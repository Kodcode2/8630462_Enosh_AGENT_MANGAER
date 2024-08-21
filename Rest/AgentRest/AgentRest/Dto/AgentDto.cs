namespace AgentRest.Dto
{
    public class AgentDto
    {
        public string Nickname { get; set; }
        public string Photo_url { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Direction Direction { get; set; }
    }
}
