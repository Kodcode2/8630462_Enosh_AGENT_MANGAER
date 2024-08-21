namespace AgentRest.Utils
{
    public class DistanceCalculation
    {
        // Calculate the distance between the two points.
        public static double CalculateDistance(int agentX, int agentY, int targetX, int targetY) =>
            Math.Sqrt(Math.Pow(targetX - agentX, 2) + Math.Pow(targetY - agentY, 2));

        // Calculate how much action time is left in hours.
        public static double CalculationOperationTime(double distance) =>
            distance / 5;


    }
}
