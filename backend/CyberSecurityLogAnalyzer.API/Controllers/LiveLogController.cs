using Microsoft.AspNetCore.Mvc;
using CyberSecurityLogAnalyzer.Core.Services;

namespace CyberSecurityLogAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiveLogController : ControllerBase
    {
        private readonly LiveLogService _liveLogService;

        public LiveLogController()
        {
            _liveLogService = new LiveLogService();
        }

        // GET api/livelog?count=10
        [HttpGet]
        public IActionResult GetRandomLogs([FromQuery] int count = 10)
        {
            var logs = _liveLogService.GenerateRandomLogs(count);
            return Ok(logs);
        }
    }
}
