using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public class CurrencyPerformanceVMMapper : PerformanceVMMapper, ICurrencyPerformanceVMMapper
    {
        public CurrencyPerformanceVMMapper(
            ICalculationService calculationService
            )
            : base(calculationService)
        {
        }
    }
}