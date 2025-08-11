namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    using Common.Services;

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