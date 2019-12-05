using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class RawDataService : IHandleRawData
    {
        public void HandleRawData<T>(IEnumerable<IEnumerable<T>> listData, IEnumerable<IHandleData> handlers)
        {
            if (listData == null || !listData.Any())
            {
                return;
            }

            foreach (var data in listData)
            {
                HandleRawData(data, handlers);
            }
        }

        public void HandleRawData<T>(IEnumerable<T> listData, IEnumerable<IHandleData> handlers)
        {
            if (listData == null || !listData.Any())
            {
                return;
            }

            foreach (var data in listData)
            {
                foreach (var handler in handlers)
                {
                    handler.HandleData(data);
                }
            }
        }
    }
}