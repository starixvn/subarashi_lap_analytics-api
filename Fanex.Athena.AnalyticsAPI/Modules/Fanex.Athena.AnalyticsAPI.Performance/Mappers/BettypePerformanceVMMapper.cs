using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Fanex.Athena.AnalyticsAPI.Performance.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    public class BettypePerformanceVMMapper : PerformanceVMMapper, IBettypePerformanceVMMapper
    {
        public BettypePerformanceVMMapper(
            ICalculationService calculationService
            )
            : base(calculationService)
        {
        }
    }
}