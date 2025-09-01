using Microsoft.AspNetCore.Mvc;
using CyberSecurityLogAnalyzer.Core.Services;
using MLNetRiskPredictor;

namespace CyberSecurityLogAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : ControllerBase
    {
        private readonly RiskScoreService _riskScoreService;

        public PredictionController(RiskScoreService riskScoreService)
        {
            _riskScoreService = riskScoreService;
        }

        [HttpGet("test-ddos")]
        public IActionResult TestDDoS()
        {
            // ModelInput kullanıyoruz
            var ddosSample = new ModelInput
            {
                DestinationPort = 80,
                FlowDuration = 1293792,
                TotalFwdPackets = 3,
                TotalBackwardPackets = 7,
                TotalLengthFwdPackets = 26,
                TotalLengthBwdPackets = 11607,
                FwdPacketLengthMax = 20,
                FwdPacketLengthMin = 0,
                FwdPacketLengthMean = 8.666666667f,
                FwdPacketLengthStd = 10.26320288f,
            };

            var score = _riskScoreService.PredictRiskScore(ddosSample);

            return Ok(new
            {
                PredictionScore = score
            });
        }
    }
}
