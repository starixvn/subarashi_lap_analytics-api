using Fanex.Athena.AnalyticsAPI.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Common.Mappers
{
    public class ViewModelMapper
    {
        protected const string DISTRIBUTION_OTHERS_ID = "0";
        protected const string DISTRIBUTION_OTHERS_NAME = "Others";
        protected const string CURRENT_PERIOD_NAME = "Current Period";
        protected const string PREVIOUS_PERIOD_NAME = "Previous Period";

        protected readonly ICalculationService _calculationService;

        public ViewModelMapper(
            ICalculationService calculationService
            )
        {
            _calculationService = calculationService;
        }

        protected static bool HasOthers(IEnumerable<(string, string)> topDistributions)
        {
            if (topDistributions == null)
            {
                throw new ArgumentNullException(nameof(topDistributions));
            }
            return topDistributions.Any(_ => _.Item1 == DISTRIBUTION_OTHERS_ID);
        }
    }
}