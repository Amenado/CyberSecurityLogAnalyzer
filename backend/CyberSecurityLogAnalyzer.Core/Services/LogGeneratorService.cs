using System;
using System.Collections.Generic;
using CyberSecurityLogAnalyzer.Core.Services;

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

public class LogGeneratorService
{
    private readonly RiskPredictionService _riskPredictionService;
    private readonly Random _random = new Random();

    public LogGeneratorService()
    {
        _riskPredictionService = new RiskPredictionService();
    }

    public List<LiveLog> GenerateRandomLogs(int count)
    {
        var logs = new List<LiveLog>();

        for (int i = 0; i < count; i++)
        {
            var modelInput = new ModelInput
            {
                DestinationPort = _random.Next(-1, 65001),
                FlowDuration = _random.Next(-2, 120000001),
                TotalFwdPackets = _random.Next(0, 1931),
                TotalBackwardPackets = _random.Next(-1, 2941),
                TotalLengthofFwdPackets = _random.Next(-1, 183001),
                TotalLengthofBwdPackets = _random.Next(-1, 5170001),
                FwdPacketLengthMax = _random.Next(-1, 11701),
                FwdPacketLengthMin = _random.Next(-1, 1473),
                FwdPacketLengthMean = _random.Next(-1, 3871),
                FwdPacketLengthStd = _random.Next(-1, 6691)
            };

            float riskScore = _riskPredictionService.PredictRiskScore(modelInput);

            logs.Add(new LiveLog
            {
                Timestamp = DateTime.Now,
                SourceIP = $"192.168.{_random.Next(0, 255)}.{_random.Next(0, 255)}",
                DestinationIP = $"10.0.{_random.Next(0, 255)}.{_random.Next(0, 255)}",
                Action = _random.Next(0, 2) == 0 ? "ALLOW" : "DENY",
                Status = _random.Next(0, 2) == 0 ? "SUCCESS" : "FAILED",
                RiskScore = riskScore,
                Server = $"server-{_random.Next(1, 5)}"
            });
        }

        return logs;
    }
}
