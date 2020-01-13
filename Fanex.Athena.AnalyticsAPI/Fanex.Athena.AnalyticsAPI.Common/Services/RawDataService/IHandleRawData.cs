using System.Collections.Generic;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface IHandleRawData
    {
        void HandleRawData<T>(IEnumerable<IEnumerable<T>> listData, IEnumerable<IHandleData> handlers);

        void HandleRawData<T>(IEnumerable<T> listData, IEnumerable<IHandleData> handlers);
    }
}