using Microsoft.AspNetCore.Mvc;
using CyberSecurityLogAnalyzer.Core.Services;
using System.Collections.Generic;

namespace CyberSecurityLogAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiveLogsController : ControllerBase
    {
        private readonly LogGeneratorService _logGeneratorService;

        public LiveLogsController()
        {
            _logGeneratorService = new LogGeneratorService();
        }

        [HttpGet("logs")]
        public IActionResult GetLogs(int count = 10)
        {
            var logs = _logGeneratorService.GenerateRandomLogs(count);
            return Ok(logs);
        }
    }
}