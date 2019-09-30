using Fanex.Athena.AnalyticsAPI.Common.Services;
using System.Collections.Generic;
using System.Linq;

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

        protected virtual bool IsInvalidDto<TDto>(params IEnumerable<TDto>[] dtos)
        {
            return dtos == null || dtos.Count() < 2;
        }
    }
}