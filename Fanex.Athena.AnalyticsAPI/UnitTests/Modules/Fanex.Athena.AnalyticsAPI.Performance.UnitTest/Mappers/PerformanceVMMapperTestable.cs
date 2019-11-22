using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Mappers
{
    public class DistributionTestClass : DistributionDTO
    {
        public string TestId { get; set; }

        public string TestName { get; set; }

        public override string Id => TestId;

        public override string Name => TestName;
    }

    public class MeasurementPeriodDTOTestClass : MeasurementPeriodDTO
    {
        public override string Id => throw new System.NotImplementedException();

        public override string Name => throw new System.NotImplementedException();
    }
}