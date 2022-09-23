using Hangfire.Demo.Models;

namespace Hangfire.Demo.Services
{
    public class LongRunningTaskService
    {
        public async Task<string> LongRunningTask(int ms)
        {
            Thread.Sleep(ms);
            return $"Task is done in {ms} milliseconds.";
        }

        public List<TaskObject> LongRunningTaskList(int ms)
        {
            var response = new List<TaskObject>
            {
                new TaskObject { Id = 1, Text = "text 1" },
                new TaskObject { Id = 2, Text = "text 2" },
                new TaskObject { Id = 3, Text = "text 3" }
            };

            Thread.Sleep(ms);
            return response;
        }

        public void LongRunningTaskVoid(int ms)
        {
            Thread.Sleep(ms);
        }
    }
}
