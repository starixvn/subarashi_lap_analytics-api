namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public interface IHandleData
    {
        void HandleData<T>(T data);
    }
}