using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class CalculationService : ICalculationService
    {
        private static int Round_Floating_Point = 2;

        public double CalculateGrowth(double? later, double? previous)
        {
            later = later ?? 0;
            previous = previous ?? 0;

            if(previous.Equals(0))
            {
                return 0;
            }

            var result = (later.Value - previous.Value) / Math.Abs(previous.Value);
            result = Math.Round(result, Round_Floating_Point);

            return result;
        }
    }
}