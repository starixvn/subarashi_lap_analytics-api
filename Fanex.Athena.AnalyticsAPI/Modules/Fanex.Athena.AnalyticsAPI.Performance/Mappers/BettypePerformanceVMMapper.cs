using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
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