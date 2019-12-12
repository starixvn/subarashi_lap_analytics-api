using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Common.Mappers
{
    public class ViewModelMapper
    {
        public const string DISTRIBUTION_OTHERS_ID = "0";
        public const string DISTRIBUTION_OTHERS_NAME = "Others";

        protected const string CURRENT_PERIOD_ID = "current_period";
        protected const string PREVIOUS_PERIOD_ID = "previous_period";
        protected const string CURRENT_PERIOD_NAME = "Current Period";
        protected const string PREVIOUS_PERIOD_NAME = "Previous Period";

        protected readonly ICalculationService _calculationService;

        public ViewModelMapper(
            ICalculationService calculationService
            )
        {
            _calculationService = calculationService;
        }
    }
}