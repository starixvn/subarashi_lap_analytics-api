namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface ICalculationService
    {
        double CalculateGrowth(double? later, double? previous);
    }
}