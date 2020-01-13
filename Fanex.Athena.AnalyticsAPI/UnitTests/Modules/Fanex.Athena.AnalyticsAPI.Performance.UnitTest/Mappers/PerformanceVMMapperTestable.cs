using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Mappers
{
    internal class DistributionDtoTestClass : DistributionDTO
    {
        public string TestId { get; set; }

        public string TestName { get; set; }

        public override string Id => TestId;

        public override string Name => TestName;
    }

    internal class MeasurementPerioddDtoTestClass : MeasurementPeriodDTO
    {
        public string TestId { get; set; }

        public string TestName { get; set; }

        public override string Id => TestId;

        public override string Name => TestName;
    }

    internal class MeasurementComparisonDtoTestClass : MeasurementComparisonDTO
    {
        public string TestId { get; set; }

        public string TestName { get; set; }

        public override string Id => TestId;

        public override string Name => TestName;
    }

    internal class TopChangeDtoTestClass : TopChangeDTO
    {
        public string TestId { get; set; }

        public string TestName { get; set; }

        public override string Id => TestId;

        public override string Name => TestName;
    }
}