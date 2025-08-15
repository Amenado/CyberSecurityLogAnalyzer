using Microsoft.AspNetCore.Mvc;
using CyberSecurityLogAnalyzer.Core.Services;
using CyberSecurityLogAnalyzer.API.Models;

namespace CyberSecurityLogAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiveLogsController : ControllerBase
    {
        private readonly RiskScoreService _riskScoreService;

        public LiveLogsController(RiskScoreService riskScoreService)
        {
            _riskScoreService = riskScoreService;
        }

        [HttpGet("random")]
        public IActionResult GetRandomLogs(int count = 10)
        {
            var logs = new List<LiveLog>();

            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                // Random parametreler
                var logInput = new Core.Services.NetworkLogInput
                {
                    DestinationPort = random.Next(-1, 65001),
                    FlowDuration = random.Next(-2, 120000001),
                    TotalFwdPackets = random.Next(0, 1931),
                    TotalBackwardPackets = random.Next(-1, 2941),
                    TotalLengthOfFwdPackets = random.Next(-1, 183001),
                    TotalLengthOfBwdPackets = random.Next(-1, 5170001),
                    FwdPacketLengthMax = random.Next(-1, 11701),
                    FwdPacketLengthMin = random.Next(-1, 1473),
                    FwdPacketLengthMean = random.Next(-1, 3871),
                    FwdPacketLengthStd = random.Next(-1, 6691)
                };

                float riskScore = _riskScoreService.PredictRiskScore(logInput);

                logs.Add(new LiveLog
                {
                    Timestamp = DateTime.UtcNow,
                    SourceIP = $"192.168.{random.Next(0, 255)}.{random.Next(0, 255)}",
                    DestinationIP = $"10.0.{random.Next(0, 255)}.{random.Next(0, 255)}",
                    Action = random.Next(0, 2) == 0 ? "Allow" : "Block",
                    Status = random.Next(0, 2) == 0 ? "Success" : "Failed",
                    Server = $"Server-{random.Next(1, 10)}",
                    RiskScore = riskScore
                });
            }

            return Ok(logs);
        }
    }

    public class LiveLog
    {
        public DateTime Timestamp { get; set; }
        public string SourceIP { get; set; }
        public string DestinationIP { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public float RiskScore { get; set; }
        public string Server { get; set; }
    }
}
