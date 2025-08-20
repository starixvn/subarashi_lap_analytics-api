namespace Fanex.Athena.AnalyticsAPI.Common.Extensions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    public static class TaskExtension
    {
        public static async Task<(T1, T2)> WhenAllAsync<T1, T2>(Task<T1> task1, Task<T2> task2)
        {
            await Task.WhenAll(task1, task2);
            return (await task1, await task2);
        }

        public static async Task<(T1, T2, T3)> WhenAllAsync<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Task<T3> task3)
        {
            await Task.WhenAll(task1, task2, task3);
            return (await task1, await task2, await task3);
        }

        [SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "<Pending>")]
        public static async Task<(T1, T2, T3, T4)> WhenAllAsync<T1, T2, T3, T4>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4)
        {
            await Task.WhenAll(task1, task2, task3, task4);
            return (await task1, await task2, await task3, await task4);
        }
    }
}