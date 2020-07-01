using System;
using System.Threading;

namespace ThreadPoolExercises.Core
{
    public class ThreadingHelpers
    {
        public static void ExecuteOnThread(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            var thread = new Thread(() => DoWork(action, repeats, token, errorAction));
            thread.Start();
            thread.Join();
        }

        public static void ExecuteOnThreadPool(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            // * Queue work item to a thread pool that executes `action` given number of `repeats` - waiting for the execution!
            //   HINT: you may use `AutoResetEvent` to wait until the queued work item finishes
            // * In a loop, check whether `token` is not cancelled
            // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

            var evt = new AutoResetEvent(false);
            ThreadPool.RegisterWaitForSingleObject(evt, (a, b) =>
            {
                DoWork(action, repeats, token, errorAction);
                evt.Set();
            }, null, 0, true);

            evt.WaitOne();
        }

        static void DoWork(Action action, int repeats, CancellationToken token, Action<Exception>? errorAction)
        {
            try
            {
                var i = 0;
                while (i < repeats)
                {
                    token.ThrowIfCancellationRequested();
                    action();
                    i++;
                }
            }
            catch (Exception ex)
            {
                errorAction?.Invoke(ex);
            }
        }
    }
}
