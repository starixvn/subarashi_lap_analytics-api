namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ICalculationService
    {
        double CalculateGrowth(double? current, double? previous);

        double CalculateDistribution(double? value, double total);

        double ToRound(double? value, int numberOfRound = 2);
    }
}