using Fanex.Athena.AnalyticsAPI.Common.Models.MetaModels;
using Newtonsoft.Json;
using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Helpers
{
    public static class MetaModelHelper
    {
        public static TMetalModel BackToCurrentPeriodByMonth<TMetalModel>(TMetalModel metaModel, int numberOfMonth)
            where TMetalModel : GeneralFilterMetaModel
        {
            var modelClone = metaModel.Clone();

            modelClone.CurrentPeriod.FromMonth = DateTimeHelper.ParseFromFormatDateToDate(modelClone.CurrentPeriod.FromMonth)
                .AddMonths(-numberOfMonth)
                .ToString(DateTimeHelper.MONTH_FORMAT);

            return modelClone;
        }

        public static MonthRangeMetaModel GetPrevOfPrevPeriod(MonthRangeMetaModel prevPeriod, MonthRangeMetaModel currentPeriod)
        {
            var prevPeriodFrom = DateTimeHelper.ParseFromFormatDateToDate(prevPeriod.FromMonth);
            var prevPeriodTo = DateTimeHelper.ParseFromFormatDateToDate(prevPeriod.ToMonth);

            var currentPeriodFrom = DateTimeHelper.ParseFromFormatDateToDate(currentPeriod.FromMonth);
            var currentPeriodTo = DateTimeHelper.ParseFromFormatDateToDate(currentPeriod.ToMonth);

            var distanceOfTwoPeriod = DateTimeHelper.GetDistanceBetweenTwoDate(prevPeriodTo, currentPeriodFrom);
            var durationOfPeriod = DateTimeHelper.GetDistanceBetweenTwoDate(currentPeriodFrom, currentPeriodTo);

            var prevOfPrevPeriodTo = prevPeriodFrom.AddMonths(-distanceOfTwoPeriod);
            var prevOfPrevPeriodFrom = prevOfPrevPeriodTo.AddMonths(-durationOfPeriod);

            return new MonthRangeMetaModel
            {
                FromMonth = prevOfPrevPeriodFrom.ToString(DateTimeHelper.DATE_FORMAT),
                ToMonth = prevOfPrevPeriodTo.ToString(DateTimeHelper.DATE_FORMAT),
            };
        }

        public static T Clone<T>(this T source)
        {
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}