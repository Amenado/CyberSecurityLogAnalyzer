using System;
using System.Collections.Generic;
using CyberSecurityLogAnalyzer.Core.Models;
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
            var modelInput = new ModelInput();
            // saldırı trafiği için %20
            if (_random.Next(1, 101) <= 20)
            {
                modelInput.DestinationPort = _random.Next(0, 50000);
                modelInput.FlowDuration = 6308970;
                modelInput.TotalFwdPackets = 4;
                modelInput.TotalBackwardPackets = 0;
                modelInput.TotalLengthFwdPackets = 24;
                modelInput.TotalLengthBwdPackets = 0;
                modelInput.FwdPacketLengthMax = 6;
                modelInput.FwdPacketLengthMin = 6;
                modelInput.FwdPacketLengthMean = 6.0f;
                modelInput.FwdPacketLengthStd = 0.0f;
                modelInput.BwdPacketLengthMax = _random.Next(0, 12000);
                modelInput.BwdPacketLengthMin = 0;
                modelInput.BwdPacketLengthMean = _random.Next(0, 6000);
                modelInput.BwdPacketLengthStd = _random.Next(0, 8200);
                modelInput.FlowBytesPerSec = 3.804107485f;
                modelInput.FlowPacketsPerSec = 0.634017914f;
                modelInput.FlowIATMean = 2102990.0f;
                modelInput.FlowIATStd = 3642482.064f;
                modelInput.FlowIATMax = 6308966;
                modelInput.FlowIATMin = 1;
                modelInput.FwdIATTotal = 6308970;
                modelInput.FwdIATMean = 2102990.0f;
                modelInput.FwdIATStd = 3642482.064f;
                modelInput.FwdIATMax = 6308966;
                modelInput.FwdIATMin = 1;
                modelInput.BwdIATTotal = 0;
                modelInput.BwdIATMean = 0.0f;
                modelInput.BwdIATStd = 0.0f;
                modelInput.BwdIATMax = 0;
                modelInput.BwdIATMin = 0;
                modelInput.FwdPSHFlags = 0;
                modelInput.FwdHeaderLength = 80;
                modelInput.BwdHeaderLength = 0;
                modelInput.FwdPacketsPerSec = 0.634017914f;
                modelInput.BwdPacketsPerSec = 0.0f;
                modelInput.MinPacketLength = 6;
                modelInput.MaxPacketLength = 6;
                modelInput.PacketLengthMean = 6.0f;
                modelInput.PacketLengthStd = 0.0f;
                modelInput.PacketLengthVariance = 0.0f;
                modelInput.FINFlagCount = 0;
                modelInput.SYNFlagCount = 0;
                modelInput.RSTFlagCount = 0;
                modelInput.PSHFlagCount = 0;
                modelInput.ACKFlagCount = 1;
                modelInput.URGFlagCount = 0;
                modelInput.ECEFlagCount = 0;
                modelInput.DownUpRatio = 0;
                modelInput.AveragePacketSize = 7.5f;
                modelInput.AvgFwdSegmentSize = 6.0f;
                modelInput.AvgBwdSegmentSize = _random.Next(0, 6000);
                modelInput.FwdHeaderLength2 = 80;
                modelInput.SubflowFwdPackets = 4;
                modelInput.SubflowFwdBytes = 24;
                modelInput.SubflowBwdPackets = 0;
                modelInput.SubflowBwdBytes = 0;
                modelInput.InitWinBytesForward = 256;
                modelInput.InitWinBytesBackward = -1;
                modelInput.ActDataPktFwd = 3;
                modelInput.MinSegSizeForward = 20;
                modelInput.ActiveMean = 4.0f;
                modelInput.ActiveStd = 0.0f;
                modelInput.ActiveMax = 4;
                modelInput.ActiveMin = 4;
                modelInput.IdleMean = 6308966.0f;
                modelInput.IdleStd = 0.0f;
                modelInput.IdleMax = 6308966;
                modelInput.IdleMin = 6308966;

                var log = new LiveLog
                {
                    Timestamp = DateTime.Now,
                    SourceIP = $"192.168.{_random.Next(0, 255)}.{_random.Next(0, 255)}",
                    DestinationIP = $"10.0.{_random.Next(0, 255)}.{_random.Next(0, 255)}",
                    Action = "DENY",
                    Status = "FAILED",
                    Server = $"server-{_random.Next(1, 5)}"
                };
                
                float riskScore = _riskPredictionService.PredictRiskScore(modelInput);
                log.RiskScore = riskScore;
                logs.Add(log);
            }
            else
            {
                modelInput.DestinationPort = _random.Next(0, 65535);
                modelInput.FlowDuration = _random.Next(0, 100);
                modelInput.TotalFwdPackets = _random.Next(0, 100);
                modelInput.TotalBackwardPackets = _random.Next(0, 100);
                modelInput.TotalLengthFwdPackets = _random.Next(0, 100);
                modelInput.TotalLengthBwdPackets = _random.Next(0, 100);
                modelInput.FwdPacketLengthMax = _random.Next(0, 100);
                modelInput.FwdPacketLengthMin = _random.Next(0, 100);
                modelInput.FwdPacketLengthMean = _random.Next(0, 100);
                modelInput.FwdPacketLengthStd = _random.Next(0, 100);
                modelInput.BwdPacketLengthMax = _random.Next(0, 100);
                modelInput.BwdPacketLengthMin = _random.Next(0, 100);
                modelInput.BwdPacketLengthMean = _random.Next(0, 100);
                modelInput.BwdPacketLengthStd = _random.Next(0, 100);
                modelInput.FlowBytesPerSec = _random.Next(0, 100);
                modelInput.FlowPacketsPerSec = _random.Next(0, 100);
                modelInput.FlowIATMean = _random.Next(0, 100);
                modelInput.FlowIATStd = _random.Next(0, 100);
                modelInput.FlowIATMax = _random.Next(0, 100);
                modelInput.FlowIATMin = _random.Next(0, 100);
                modelInput.FwdIATTotal = _random.Next(0, 100);
                modelInput.FwdIATMean = _random.Next(0, 100);
                modelInput.FwdIATStd = _random.Next(0, 100);
                modelInput.FwdIATMax = _random.Next(0, 100);
                modelInput.FwdIATMin = _random.Next(0, 100);
                modelInput.BwdIATTotal = _random.Next(0, 100);
                modelInput.BwdIATMean = _random.Next(0, 100);
                modelInput.BwdIATStd = _random.Next(0, 100);
                modelInput.BwdIATMax = _random.Next(0, 100);
                modelInput.BwdIATMin = _random.Next(0, 100);
                modelInput.FwdPSHFlags = _random.Next(0, 100);
                modelInput.FwdHeaderLength = _random.Next(0, 100);
                modelInput.BwdHeaderLength = _random.Next(0, 100);
                modelInput.FwdPacketsPerSec = _random.Next(0, 100);
                modelInput.BwdPacketsPerSec = _random.Next(0, 100);
                modelInput.MinPacketLength = _random.Next(0, 100);
                modelInput.MaxPacketLength = _random.Next(0, 100);
                modelInput.PacketLengthMean = _random.Next(0, 100);
                modelInput.PacketLengthStd = _random.Next(0, 100);
                modelInput.PacketLengthVariance = _random.Next(0, 100);
                modelInput.FINFlagCount = _random.Next(0, 100);
                modelInput.SYNFlagCount = _random.Next(0, 100);
                modelInput.RSTFlagCount = _random.Next(0, 100);
                modelInput.PSHFlagCount = _random.Next(0, 100);
                modelInput.ACKFlagCount = _random.Next(0, 100);
                modelInput.URGFlagCount = _random.Next(0, 100);
                modelInput.ECEFlagCount = _random.Next(0, 100);
                modelInput.DownUpRatio = _random.Next(0, 100);
                modelInput.AveragePacketSize = _random.Next(0, 100);
                modelInput.AvgFwdSegmentSize = _random.Next(0, 100);
                modelInput.AvgBwdSegmentSize = _random.Next(0, 100);
                modelInput.FwdHeaderLength2 = _random.Next(0, 100);
                modelInput.SubflowFwdPackets = _random.Next(0, 100);
                modelInput.SubflowFwdBytes = _random.Next(0, 100);
                modelInput.SubflowBwdPackets = _random.Next(0, 100);
                modelInput.SubflowBwdBytes = _random.Next(0, 100);
                modelInput.InitWinBytesForward = _random.Next(0, 100);
                modelInput.InitWinBytesBackward = _random.Next(0, 100);
                modelInput.ActDataPktFwd = _random.Next(0, 100);
                modelInput.MinSegSizeForward = _random.Next(0, 100);
                modelInput.ActiveMean = _random.Next(0, 100);
                modelInput.ActiveStd = _random.Next(0, 100);
                modelInput.ActiveMax = _random.Next(0, 100);
                modelInput.ActiveMin = _random.Next(0, 100);
                modelInput.IdleMean = _random.Next(0, 100);
                modelInput.IdleStd = _random.Next(0, 100);
                modelInput.IdleMax = _random.Next(0, 100);
                modelInput.IdleMin = _random.Next(0, 100);

                var log = new LiveLog
                {
                    Timestamp = DateTime.Now,
                    SourceIP = $"192.168.{_random.Next(0, 255)}.{_random.Next(0, 255)}",
                    DestinationIP = $"10.0.{_random.Next(0, 255)}.{_random.Next(0, 255)}",
                    Action = "ALLOW",
                    Status = "SUCCESS", 
                    Server = $"server-{_random.Next(1, 50)}"
                };
                
                float riskScore = _riskPredictionService.PredictRiskScore(modelInput);
                log.RiskScore = riskScore;
                logs.Add(log);
            }
        }
        return logs;
    }
}