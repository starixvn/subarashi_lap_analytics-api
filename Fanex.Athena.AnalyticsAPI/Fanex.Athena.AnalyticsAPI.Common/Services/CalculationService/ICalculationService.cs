namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ICalculationService
    {
        double CalculateGrowth(double? current, double? previous);
    }
}