namespace Fanex.Athena.AnalyticsAPI.Performance.Mappers
{
    using System;
    using System.Collections.Generic;
    using Models.DTO;
    using Models.MetaModels;
    using Models.ViewModels;

    public interface IPerformanceVMMapper
    {
        IEnumerable<MeasurementPeriodComparisonGrowthVM> ToMeasurementGrowthPeriodVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<DistributionMetaModel> topDistributions)
            where T : MeasurementPeriodDTO;

        IEnumerable<MeasurementGrowthVM> ToMeasurementGrowthTimeVM(IEnumerable<MeasurementTimeDTO> periodDtos, DateTime fromMonth, DateTime toMonth);

        IEnumerable<DistributionVM> ToDistributionVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<DistributionMetaModel> topDistributions)
            where T : DistributionDTO;

        IEnumerable<MeasurementComparisonVM> ToMeasurementComparisonVM<T>(
            IEnumerable<T> prevPeriodDtos,
            IEnumerable<T> currentPeriodDtos,
            IEnumerable<DistributionMetaModel> topDistributions)
            where T : MeasurementComparisonDTO;

        IEnumerable<OverallMeasurementVM> ToOverallMeasurementVM<T>(T prevPeriodDto, T currentPeriodDto)
            where T : TotalDTO;
    }
}