namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    using Common.Services;

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