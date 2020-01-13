using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
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