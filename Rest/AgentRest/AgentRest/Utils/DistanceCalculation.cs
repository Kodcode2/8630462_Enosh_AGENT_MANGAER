﻿namespace AgentRest.Utils
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
            string yDirectin = agentX switch
            {
                _ when agentY < targetY => "n",
                _ when agentY > targetY => "s",
                _ => string.Empty
            };
            string xDirectin = agentX switch
            {
                _ when agentX > targetX => "w",
                _ when agentX < targetX => "e",
                _ => string.Empty
            };

            return $"{yDirectin}{xDirectin}";
        }


    }
}
