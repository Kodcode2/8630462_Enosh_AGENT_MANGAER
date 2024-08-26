using Humanizer;

namespace AgentClient.ViewModel
{
    public class MissionsManagementVM
    {
        public int MissionId { get; set; }

        public string AgentName { get; set; }
        public int AgentLocationX { get; set; }
        public int AgentLocationY { get; set; }

        public string TargetName { get; set; }
        public string TargetRole { get; set; }
        public int TargetLocationX { get; set; }
        public int TargetLocationY { get; set; }

        public double Distance { get; set; }
        public double TimeToEliminate { get; set; }
    }
}
