using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class MarginHandler : IHandleData
    {
        private const int PERCENT_VALUE = 100;

        public void HandleData<T>(T data)
        {
            var totalObject = data as IMeasurement;
            if (totalObject == null)
            {
                return;
            }
            totalObject.Total *= PERCENT_VALUE;
        }
    }
}