using Fanex.Athena.AnalyticsAPI.Common.Services;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Common.Helpers
{
    public class ViewModelHelper
    {
        protected readonly ICalculationService _calculationService;

        public ViewModelHelper(
            ICalculationService calculationService
            )
        {
            _calculationService = calculationService;
        }

        protected virtual bool IsInValidDto<TDto>(params IEnumerable<TDto>[] dtos)
        {
            return dtos == null || dtos.Count() < 2;
        }
    }
}