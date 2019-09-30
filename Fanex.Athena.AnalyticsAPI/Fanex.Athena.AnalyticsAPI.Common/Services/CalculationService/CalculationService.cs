using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class CalculationService : ICalculationService
    {
        private static int Round_Floating_Point = 2;
        private static int Thousand_Unit = 1000;

        public double CalculateGrowth(double? current, double? previous)
        {
            current = current ?? 0;
            previous = previous ?? 0;

            if(previous == 0)
            {
                return 0;
            }

            var result = ((current.Value - previous.Value) / Math.Abs(previous.Value))*100;
            result = ToRound(result, Round_Floating_Point);

            return result;
        }

        public double ToRound(double? value, int numberOfRound = 2)
        {
            return Math.Round(value ?? 0, numberOfRound);
        }

        public double ToThousandUnit(double? value)
        {
            value = value ?? 0;

            return Math.Round(value.Value / Thousand_Unit, Round_Floating_Point);
        }
    }
}