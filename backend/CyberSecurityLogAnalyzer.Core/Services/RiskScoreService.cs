using Microsoft.ML;
using Microsoft.ML.Data;
using System.IO;

namespace CyberSecurityLogAnalyzer.Core.Services
{
    public class RiskScoreService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly PredictionEngine<NetworkLogInput, NetworkLogPrediction> _predictionEngine;

        public RiskScoreService()
        {
            _mlContext = new MLContext();
            string modelPath = @"C:\Users\BoraErki\Desktop\CyberSecurityLogAnalyzer\MLNetRiskPredictor\RiskScoreModel.zip";

            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"Model dosyası bulunamadı: {modelPath}");

            _model = _mlContext.Model.Load(modelPath, out _);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<NetworkLogInput, NetworkLogPrediction>(_model);
        }

        // Artık tek bir float dönecek
        public float PredictRiskScore(NetworkLogInput input)
        {
            var prediction = _predictionEngine.Predict(input);
            return prediction.Probability;
            // 0-1 arası gerçek risk skoru
        }
    }

    public class NetworkLogInput
    {
        [ColumnName("DestinationPort")]
        public float DestinationPort { get; set; }

        [ColumnName("FlowDuration")]
        public float FlowDuration { get; set; }

        [ColumnName("TotalFwdPackets")]
        public float TotalFwdPackets { get; set; }

        [ColumnName("TotalBackwardPackets")]
        public float TotalBackwardPackets { get; set; }

        [ColumnName("TotalLengthFwdPackets")]
        public float TotalLengthOfFwdPackets { get; set; }

        [ColumnName("TotalLengthBwdPackets")]
        public float TotalLengthOfBwdPackets { get; set; }

        [ColumnName("FwdPacketLengthMax")]
        public float FwdPacketLengthMax { get; set; }

        [ColumnName("FwdPacketLengthMin")]
        public float FwdPacketLengthMin { get; set; }

        [ColumnName("FwdPacketLengthMean")]
        public float FwdPacketLengthMean { get; set; }

        [ColumnName("FwdPacketLengthStd")]
        public float FwdPacketLengthStd { get; set; }

        [ColumnName("Label")]
        public string Label { get; set; }
    }

    public class NetworkLogPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }

        [ColumnName("Probability")] // artık tek float
        public float Probability { get; set; }
    }
}
