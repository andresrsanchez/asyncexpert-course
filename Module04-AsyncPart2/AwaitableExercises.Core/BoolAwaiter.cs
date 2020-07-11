using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AwaitableExercises.Core
{
    public static class BoolExtensions
    {
        public static TaskAwaiter<bool> GetAwaiter(this bool value)
        {
            return Task.FromResult(value).GetAwaiter();
        }

        public static TaskAwaiter<TaskAwaiter<bool>> GetAwaiter(this TaskAwaiter<bool> value)
        {
            return Task.FromResult(value).GetAwaiter();
        }
    }

    public class BoolAwaiter : INotifyCompletion
    {
        public void OnCompleted(Action continuation)
        {
            continuation();
        }
    }
}
