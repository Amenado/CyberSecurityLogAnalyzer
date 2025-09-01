using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace CyberSecurityLogAnalyzer.Core
{
    public class NetworkLogInput
    {
        [LoadColumn(0)]
        public float DestinationPort { get; set; }

        [LoadColumn(1)]
        public float FlowDuration { get; set; }

        [LoadColumn(2)]
        public float TotalFwdPackets { get; set; }

        [LoadColumn(3)]
        public float TotalBackwardPackets { get; set; }

        [LoadColumn(4)]
        public float TotalLengthFwdPackets { get; set; }

        [LoadColumn(5)]
        public float TotalLengthBwdPackets { get; set; }

        [LoadColumn(6)]
        public float FwdPacketLengthMax { get; set; }

        [LoadColumn(7)]
        public float FwdPacketLengthMin { get; set; }

        [LoadColumn(8)]
        public float FwdPacketLengthMean { get; set; }

        [LoadColumn(9)]
        public float FwdPacketLengthStd { get; set; }
    }

    public class NetworkLogPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }

        [ColumnName("Score")]
        public float[] Score { get; set; }
    }

    public class RandomNetworkLogGenerator
    {
        private readonly MLContext mlContext;
        private readonly ITransformer mlModel;
        private readonly PredictionEngine<NetworkLogInput, NetworkLogPrediction> predEngine;
        private readonly Random random;

        public RandomNetworkLogGenerator(string modelPath)
        {
            mlContext = new MLContext();
            mlModel = mlContext.Model.Load(modelPath, out _);
            predEngine = mlContext.Model.CreatePredictionEngine<NetworkLogInput, NetworkLogPrediction>(mlModel);
            random = new Random();
        }

        public GeneratedLog Generate()
        {
            var input = new NetworkLogInput
            {
                DestinationPort = random.Next(1, 65535),
                FlowDuration = (float)random.NextDouble() * 10000,
                TotalFwdPackets = random.Next(1, 500),
                TotalBackwardPackets = random.Next(1, 500),
                TotalLengthFwdPackets = (float)random.NextDouble() * 1500,
                TotalLengthBwdPackets = (float)random.NextDouble() * 1500,
                FwdPacketLengthMax = (float)random.NextDouble() * 1500,
                FwdPacketLengthMin = (float)random.NextDouble() * 1500,
                FwdPacketLengthMean = (float)random.NextDouble() * 1500,
                FwdPacketLengthStd = (float)random.NextDouble() * 500
            };

            var prediction = predEngine.Predict(input);
            float riskScore = MapLabelToScore(prediction.PredictedLabel);

            var log = new GeneratedLog
            {
                Timestamp = DateTime.Now,
                SourceIP = $"192.168.{random.Next(0, 255)}.{random.Next(0, 255)}",
                DestinationIP = $"10.0.{random.Next(0, 255)}.{random.Next(0, 255)}",
                Action = random.Next(0, 2) == 0 ? "ALLOW" : "BLOCK",
                Status = random.Next(0, 2) == 0 ? "SUCCESS" : "FAILURE",
                RiskScore = riskScore,
                Server = $"Server-{random.Next(1,5)}"
            };

            return log;
        }

        private float MapLabelToScore(string label)
        {
            return label switch
            {
                "BENIGN" => 0f,
                "Anomaly" => 1f,
                _ => 0.5f
            };
        }
    }

    public class GeneratedLog
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
