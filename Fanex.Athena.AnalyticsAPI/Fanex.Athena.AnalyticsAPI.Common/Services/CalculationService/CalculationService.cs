namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    using System;

    public class CalculationService : ICalculationService
    {
        private static int Round_Floating_Point = 2;

        public double CalculateDistribution(double? value, double total)
        {
            value ??= 0;

            if (value.Value.CompareTo(0) == 0 || total.CompareTo(0) == 0)
            {
                return 0;
            }

            var result = (value.Value / Math.Abs(total)) * 100;
            result = ToRound(result);

            return result;
        }

        public double CalculateMargin(double profit, double turnover)
        {
            if (turnover.CompareTo(0) == 0)
            {
                return 0.0;
            }
            return ToPercentUnit(profit / turnover);
        }

        public double CalculateGrowth(double? current, double? previous)
        {
            current ??= 0;
            previous ??= 0;

            var baseValue = previous.Value;
            if (previous.Value.CompareTo(0) == 0)
            {
                baseValue = current.Value.CompareTo(0) == 0 ? 1 : current.Value;
            }

            var result = ((current.Value - previous.Value) / Math.Abs(baseValue)) * 100;
            result = ToRound(result, Round_Floating_Point);

            return result;
        }

        public double ToPercentUnit(double? value)
        {
            value ??= 0;
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