namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ICalculationService
    {
        double CalculateGrowth(double? current, double? previous);

        double CalculateMargin(double profit, double turnover);

        double CalculateDistribution(double? value, double total);

        double ToPercentUnit(double? value);

        double ToRound(double? value, int numberOfRound = 2);
    }
}