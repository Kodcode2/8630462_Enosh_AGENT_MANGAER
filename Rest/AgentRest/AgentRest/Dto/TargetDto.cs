namespace AgentRest.Dto
{
    public enum Direction
    {
        nw,
        n,
        ne,
        w,
        e,
        sw,
        s,
        se
    }
    public class TargetDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Photo_url { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Direction Direction { get; set; }


    }
}
