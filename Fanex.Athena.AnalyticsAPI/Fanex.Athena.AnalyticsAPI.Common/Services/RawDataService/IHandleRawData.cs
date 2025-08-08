namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    using System.Collections.Generic;

    public interface IHandleRawData
    {
        void HandleRawData<T>(IEnumerable<IEnumerable<T>> listData, IEnumerable<IHandleData> handlers);

        void HandleRawData<T>(IEnumerable<T> listData, IEnumerable<IHandleData> handlers);
    }
}