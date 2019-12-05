namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public interface IHandleData
    {
        void HandleData<T>(T data);
    }
}