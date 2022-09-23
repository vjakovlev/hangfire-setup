using Hangfire.Demo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly LongRunningTaskService _taskService ;
        public HangfireController()
        {
            _taskService = new LongRunningTaskService();
        }

        [HttpGet("blocking-task")]
        public async Task<IActionResult> SlowTask([FromQuery] int ms = 5000)
        {
            var result = await _taskService.LongRunningTask(ms);
            return Ok(result);
        }

        [HttpGet("fire-and-forget")]
        public async Task<IActionResult> FireAndForgetJob([FromQuery] int ms = 5000)
        {
            var result = BackgroundJob.Enqueue(() => _taskService.LongRunningTaskList(ms));
            return Ok(result);
        }

        [HttpGet("delayed")]
        public async Task<IActionResult> DelayedJob([FromQuery] int min = 5, [FromQuery] int ms = 5000)
        {
            var ts = TimeSpan.FromMinutes(min);

            var result = BackgroundJob.Schedule(() => _taskService.LongRunningTaskList(ms), ts);
            return Ok(result);
        }

        [HttpGet("recurring-create")]
        public async Task<IActionResult> RecurringJobCreate([FromQuery] int ms, [FromQuery] string id)
        {
            RecurringJob.AddOrUpdate(id, () => _taskService.LongRunningTaskList(ms), Cron.Hourly);
            return Ok($"recurring job with id {id} is CREATED!");
        }

        [HttpDelete("recurring-delete")]
        public async Task<IActionResult> RecurringJobDelete([FromQuery] string id)
        {
            RecurringJob.RemoveIfExists(id);
            return Ok($"recurring job with id {id} is DELETED!");
        }

        [HttpGet("recurring-trigger-now")]
        public async Task<IActionResult> RecurringJobTriggerNow([FromQuery] string id)
        {
            RecurringJob.TriggerJob(id);
            return Ok($"recurring job with id {id} is TRIGGERED now!");
        }

        [HttpGet("simulate-batch")]
        public async Task<IActionResult> BatchJob([FromQuery] string id)
        {
            BackgroundJob.Enqueue(() => _taskService.LongRunningTaskList(20000));
            BackgroundJob.Enqueue(() => _taskService.LongRunningTaskList(19000));
            BackgroundJob.Enqueue(() => _taskService.LongRunningTaskList(18000));
            BackgroundJob.Enqueue(() => _taskService.LongRunningTaskList(17000));
            return Ok("batch is started");
        }


    }
}
