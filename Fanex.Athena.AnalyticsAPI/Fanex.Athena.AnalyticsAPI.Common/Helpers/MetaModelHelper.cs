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
            var objClone = metaModel.Clone();
            objClone.CurrentPeriod.FromMonth = DateTimeHelper.ParseFromFormatDateToDate(objClone.CurrentPeriod.FromMonth)
                .AddMonths(-numberOfMonth)
                .ToString(DateTimeHelper.MONTH_FORMAT);

            return objClone;
        }

        private static T Clone<T>(this T source)
        {
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}