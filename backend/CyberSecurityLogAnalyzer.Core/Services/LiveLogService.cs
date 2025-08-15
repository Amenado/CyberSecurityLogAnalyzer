using System;
using System.Collections.Generic;
using CyberSecurityLogAnalyzer.Core.Models;
using CyberSecurityLogAnalyzer.Core.Services; // RiskScoreService burada
using MLNetRiskPredictor;

namespace CyberSecurityLogAnalyzer.Core.Services
{
    public class LiveLogService
    {
        private readonly Random _random = new Random();
        private readonly RiskScoreService _riskScoreService = new RiskScoreService();

        private readonly string[] actions = { "ALLOW", "DENY" };
        private readonly string[] statuses = { "SUCCESS", "FAILURE" };
        private readonly string[] servers = { "Server1", "Server2", "Server3" };

        public List<LiveLog> GenerateRandomLogs(int count)
        {
            var logs = new List<LiveLog>();

            for (int i = 0; i < count; i++)
            {
                var log = new LiveLog
                {
                    Timestamp = DateTime.Now,
                    SourceIP = $"{_random.Next(1, 255)}.{_random.Next(0, 255)}.{_random.Next(0, 255)}.{_random.Next(0, 255)}",
                    DestinationIP = $"{_random.Next(1, 255)}.{_random.Next(0, 255)}.{_random.Next(0, 255)}.{_random.Next(0, 255)}",
                    Action = actions[_random.Next(actions.Length)],
                    Status = statuses[_random.Next(statuses.Length)],
                    Server = servers[_random.Next(servers.Length)]
                };

                // Core.Models.NetworkLog oluştur
                var networkLogCore = new CyberSecurityLogAnalyzer.Core.Models.NetworkLog
                {
                    DestinationPort = _random.Next(1, 65535),
                    FlowDuration = _random.Next(0, 10000),
                    TotalFwdPackets = _random.Next(0, 5000),
                    TotalBackwardPackets = _random.Next(0, 5000),
                    TotalLengthFwdPackets = _random.Next(0, 100000),
                    TotalLengthBwdPackets = _random.Next(0, 100000),
                    FwdPacketLengthMax = _random.Next(0, 1500),
                    FwdPacketLengthMin = _random.Next(0, 1500),
                    FwdPacketLengthMean = _random.Next(0, 1500),
                    FwdPacketLengthStd = _random.Next(0, 500),
                    Label = "BENIGN" // Dummy etiket
                };

                // MLNetRiskPredictor için NetworkLogInput oluştur
                var networkLogInput = new NetworkLogInput
                {
                    DestinationPort = networkLogCore.DestinationPort,
                    FlowDuration = networkLogCore.FlowDuration,
                    TotalFwdPackets = networkLogCore.TotalFwdPackets,
                    TotalBackwardPackets = networkLogCore.TotalBackwardPackets,
                    TotalLengthOfFwdPackets = networkLogCore.TotalLengthFwdPackets, // isim uyumlu
                    TotalLengthOfBwdPackets = networkLogCore.TotalLengthBwdPackets, // isim uyumlu
                    FwdPacketLengthMax = networkLogCore.FwdPacketLengthMax,
                    FwdPacketLengthMin = networkLogCore.FwdPacketLengthMin,
                    FwdPacketLengthMean = networkLogCore.FwdPacketLengthMean,
                    FwdPacketLengthStd = networkLogCore.FwdPacketLengthStd,
                    Label = networkLogCore.Label // burada doğru değişken
                };

                // RiskScore hesapla
                log.RiskScore = _riskScoreService.PredictRiskScore(networkLogInput);

                logs.Add(log);
            }

            return logs;
        }
    }
}
