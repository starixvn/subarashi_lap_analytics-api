using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class CalculationService : ICalculationService
    {
        private static int Round_Floating_Point = 2;

        public double CalculateDistribution(double? value, double total)
        {
            value = value ?? 0;

            if (value == 0 || total == 0)
            {
                return 0;
            }

            var result = (value.Value / Math.Abs(total)) * 100;
            result = ToRound(result);

            return result;
        }

        public double CalculateGrowth(double? current, double? previous)
        {
            current = current ?? 0;
            previous = previous ?? 0;

            if (previous == 0)
            {
                return 0;
            }

            var result = ((current.Value - previous.Value) / Math.Abs(previous.Value)) * 100;
            result = ToRound(result, Round_Floating_Point);

            return result;
        }

        public double ToPercentUnit(double? value)
        {
            value = value ?? 0;
            var result = value.Value * 100;
            result = ToRound(result);

            return result;
        }

        public double ToRound(double? value, int numberOfRound = 2)
        {
            return Math.Round(value ?? 0, numberOfRound);
        }
    }
}