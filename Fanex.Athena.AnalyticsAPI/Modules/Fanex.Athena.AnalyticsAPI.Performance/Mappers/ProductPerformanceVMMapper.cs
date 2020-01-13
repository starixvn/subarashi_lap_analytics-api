using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public class ProductPerformanceVMMapper : PerformanceVMMapper, IProductPerformanceVMMapper
    {
        public ProductPerformanceVMMapper(
            ICalculationService calculationService
            )
            : base(calculationService)
        {
        }
    }
}