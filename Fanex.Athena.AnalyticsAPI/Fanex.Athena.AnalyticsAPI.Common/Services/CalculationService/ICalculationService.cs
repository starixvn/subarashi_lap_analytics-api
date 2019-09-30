namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ICalculationService
    {
        double CalculateGrowth(double? current, double? previous);

        double ToThousandUnit(double? value);

        double ToRound(double? value, int numberOfRound = 2);
    }
}