namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    using Common.Services;

    public class BettypePerformanceVMMapper : PerformanceVMMapper, IBettypePerformanceVMMapper
    {
        public BettypePerformanceVMMapper(
            ICalculationService calculationService
            )
            : base(calculationService)
        {
        }
    }
}