using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Common.Mappers
{
    public class ViewModelMapper
    {
        protected readonly ICalculationService _calculationService;

        public ViewModelMapper(
            ICalculationService calculationService
            )
        {
            _calculationService = calculationService;
        }
    }
}