namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    using Common.Services;

    public class PlatformPerformanceVMMapper : PerformanceVMMapper, IPlatformPerformanceVMMapper
    {
        public PlatformPerformanceVMMapper(
            ICalculationService calculationService
            )
            : base(calculationService)
        {
        }
    }
}