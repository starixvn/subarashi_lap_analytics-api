using AutoFixture;
using Fanex.Athena.AnalyticsAPI.Common.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.Criteria;
using Fanex.Athena.AnalyticsAPI.Performance.Models.MetaModels;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Helpers
{
    internal class AnalyticCriteriaTestClass : AnalyticCriteria, IMeasurementCriteria
    {
        public int MeasurementId { get; set; }

        public override string GetSettingKey()
        {
            return string.Empty;
        }
    }

    internal class CurrencyPerformanceCriteriaTestClass : CurrencyPerformanceCriteria
    {
        public override string GetSettingKey()
        {
            return string.Empty;
        }
    }

    internal class ProductPerformanceCriteriaTestClass : ProductPerformanceCriteria
    {
        public override string GetSettingKey()
        {
            return string.Empty;
        }
    }

    internal class PlatformPerformanceCriteriaTestClass : PlatformPerformanceCriteria
    {
        public override string GetSettingKey()
        {
            return string.Empty;
        }
    }

    internal class BettypePerformanceCriteriaTestClass : BettypePerformanceCriteria
    {
        public override string GetSettingKey()
        {
            return string.Empty;
        }
    }

    public class CriteriaHelper_Test
    {
        private readonly Fixture _fixture;

        public CriteriaHelper_Test()
        {
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(1, false)]
        [InlineData(2, true)]
        public void ToPerformanceCriteria_WhenCall_ReturnCorrectResult(int measurementId, bool isCurrentPeriod)
        {
            //Arrange
            var metaModel = new PerformanceFilterMetaModel
            {
                CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
                PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
                Measurement = new Common.Models.MetaModels.SelectMetaModel { Id = measurementId },
            };
            //Act
            var result = Performance.Helpers.CriteriaHelper.ToPerformanceCriteria<AnalyticCriteriaTestClass, PerformanceFilterMetaModel>(metaModel, isCurrentPeriod);

            //Assert
            Assert.Equal(metaModel.Measurement.Id, result.MeasurementId);

            var period = isCurrentPeriod ? metaModel.CurrentPeriod : metaModel.PreviousPeriod;
            Assert.Equal(period.FromMonth, result.FromDate);
            Assert.Equal(period.ToMonth, result.ToDate);
        }

        [Theory]
        [InlineData(1, true, "a,b,c,d,e", "a", "b", "c", "d", "e")]
        [InlineData(1, false, "a", "a")]
        [InlineData(2, true, "")]
        public void ToCurrencyPerformanceCriteria_WhenCall_ReturnCorrectResult(int measurementId, bool isCurrentPeriod, string listCurrencyExpected, params string[] currencies)
        {
            //Arrange
            var metaModel = new CurrenyPerformanceFilterMetaModel
            {
                CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
                PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
                Measurement = new Common.Models.MetaModels.SelectMetaModel { Id = measurementId },
                Currencies = currencies
            };
            //Act
            var result = Performance.Helpers.CriteriaHelper.ToCurrencyPerformanceCriteria<CurrencyPerformanceCriteriaTestClass, CurrenyPerformanceFilterMetaModel>(metaModel, isCurrentPeriod);

            //Assert
            Assert.Equal(metaModel.Measurement.Id, result.MeasurementId);

            var period = isCurrentPeriod ? metaModel.CurrentPeriod : metaModel.PreviousPeriod;
            Assert.Equal(period.FromMonth, result.FromDate);
            Assert.Equal(period.ToMonth, result.ToDate);

            Assert.Equal(listCurrencyExpected, result.ListCurrency);
        }

        [Theory]
        [InlineData(1, true, "a,b,c,d,e", "a", "b", "c", "d", "e")]
        [InlineData(1, false, "a", "a")]
        [InlineData(2, true, "")]
        public void ToProductPerformanceCriteria_WhenCall_ReturnCorrectResult(int measurementId, bool isCurrentPeriod, string listProductExpected, params string[] products)
        {
            //Arrange
            var metaModel = new ProductPerformanceFilterMetaModel
            {
                CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
                PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
                Measurement = new Common.Models.MetaModels.SelectMetaModel { Id = measurementId },
                Products = products
            };

            //Act
            var result = Performance.Helpers.CriteriaHelper.ToProductPerformanceCriteria<ProductPerformanceCriteriaTestClass, ProductPerformanceFilterMetaModel>(metaModel, isCurrentPeriod);

            //Assert
            Assert.Equal(metaModel.Measurement.Id, result.MeasurementId);

            var period = isCurrentPeriod ? metaModel.CurrentPeriod : metaModel.PreviousPeriod;
            Assert.Equal(period.FromMonth, result.FromDate);
            Assert.Equal(period.ToMonth, result.ToDate);

            Assert.Equal(listProductExpected, result.ListProduct);
        }

        [Theory]
        [InlineData(1, true, "a,b,c,d,e", "a", "b", "c", "d", "e")]
        [InlineData(1, false, "a", "a")]
        [InlineData(2, true, "")]
        public void ToPlatformPerformanceCriteria_WhenCall_ReturnCorrectResult(int measurementId, bool isCurrentPeriod, string listPlatformExpected, params string[] platforms)
        {
            //Arrange
            var metaModel = new PlatformPerformanceFilterMetaModel
            {
                CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
                PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
                Measurement = new Common.Models.MetaModels.SelectMetaModel { Id = measurementId },
                Platforms = platforms
            };

            //Act
            var result = Performance.Helpers.CriteriaHelper.ToPlatformPerformanceCriteria<PlatformPerformanceCriteriaTestClass, PlatformPerformanceFilterMetaModel>(metaModel, isCurrentPeriod);

            //Assert
            Assert.Equal(metaModel.Measurement.Id, result.MeasurementId);

            var period = isCurrentPeriod ? metaModel.CurrentPeriod : metaModel.PreviousPeriod;
            Assert.Equal(period.FromMonth, result.FromDate);
            Assert.Equal(period.ToMonth, result.ToDate);

            Assert.Equal(listPlatformExpected, result.ListPlatform);
        }

        [Theory]
        [InlineData(1, true, "a,b,c,d,e", "a", "b", "c", "d", "e")]
        [InlineData(1, false, "a", "a")]
        [InlineData(2, true, "")]
        public void ToBettypePerformanceCriteria_WhenCall_ReturnCorrectResult(int measurementId, bool isCurrentPeriod, string listCombineBettypeExpected, params string[] combineIds)
        {
            //Arrange
            var metaModel = new BettypePerformanceFilterMetaModel
            {
                CurrentPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2019", ToMonth = "12/2019" },
                PreviousPeriod = new Common.Models.MetaModels.MonthRangeMetaModel { FromMonth = "01/2018", ToMonth = "12/2019" },
                Measurement = new Common.Models.MetaModels.SelectMetaModel { Id = measurementId },
                CombineIds = combineIds
            };

            //Act
            var result = Performance.Helpers.CriteriaHelper.ToBettypePerformanceCriteria<BettypePerformanceCriteriaTestClass, BettypePerformanceFilterMetaModel>(metaModel, isCurrentPeriod);

            //Assert
            Assert.Equal(metaModel.Measurement.Id, result.MeasurementId);

            var period = isCurrentPeriod ? metaModel.CurrentPeriod : metaModel.PreviousPeriod;
            Assert.Equal(period.FromMonth, result.FromDate);
            Assert.Equal(period.ToMonth, result.ToDate);

            Assert.Equal(listCombineBettypeExpected, result.ListCombineBettype);
        }
    }
}