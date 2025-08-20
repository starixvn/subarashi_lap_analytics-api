namespace Fanex.Athena.AnalyticsAPI.Common.Helpers
{
    public static class CreationalHelper
    {
        public static T Create<T>()
            where T : new()
        {
            return new T();
        }
    }
}