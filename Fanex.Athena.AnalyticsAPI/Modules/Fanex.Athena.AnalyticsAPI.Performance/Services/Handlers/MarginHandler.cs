using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class MarginHandler : IHandleData
    {
        private const int PERCENT_VALUE = 100;

        public void HandleData<T>(T data)
        {
            var measurementData = data as IMeasurement;
            if (measurementData == null)
            {
                return;
            }
            measurementData.Total *= PERCENT_VALUE;
        }
    }
}