using System.Threading.Tasks;

namespace JToolbox.Core.Extensions
{
    public static class TaskExtensions
    {
        public static void RunSync(this Task task, TaskScheduler scheduler = null)
        {
            if (scheduler != null)
            {
                task.RunSynchronously(scheduler);
            }
            else
            {
                task.RunSynchronously();
            }
            task.Wait();
        }

        public static T RunSync<T>(this Task<T> task, TaskScheduler scheduler = null)
        {
            RunSync(task, scheduler);
            return task.Result;
        }
    }
}