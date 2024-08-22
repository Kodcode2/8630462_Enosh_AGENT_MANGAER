namespace AgentRest.Utils
{
    public static class DistanceCalculation
    {
        // Calculate the distance between the two points.
        public static double CalculateDistance(int agentX, int agentY, int targetX, int targetY) =>
            Math.Sqrt(Math.Pow(targetX - agentX, 2) + Math.Pow(targetY - agentY, 2));

        // Calculate how much action time is left in hours.
        public static double CalculationOperationTime(double distance) =>
            distance / 5;

        // Calculation of which direction the agent should go.
        public static string WhichDirection(int agentX, int agentY, int targetX, int targetY)
        {
            string direction = string.Empty;
            if (agentY > targetY)
                direction += "n";
            else if (agentY < targetY)
                direction += "s";

            if (agentX > targetX) 
                direction += "w";
            else if (agentX < targetX)
                direction += "e";

            return direction;
        }
    }
}
