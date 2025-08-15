using Microsoft.ML;
using System.IO;

namespace CyberSecurityLogAnalyzer.Core.Services
{
    public class RiskPredictionService
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<ModelInput, ModelOutput> _predictionEngine;

        public RiskPredictionService()
        {
            _mlContext = new MLContext();
            string modelPath = @"C:\Users\BoraErki\Desktop\CyberSecurityLogAnalyzer\MLNetRiskPredictor\RiskScoreModel.zip";

            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"Model file not found at path: {modelPath}");

            DataViewSchema modelSchema;
            ITransformer trainedModel = _mlContext.Model.Load(modelPath, out modelSchema);

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);
        }

        public float PredictRiskScore(ModelInput input)
        {
            var prediction = _predictionEngine.Predict(input);
            return prediction.Score;
        }
    }

    // Model input class — CSV'deki sütunlara uygun
    public class ModelInput
    {
        public float DestinationPort { get; set; }
        public float FlowDuration { get; set; }
        public float TotalFwdPackets { get; set; }
        public float TotalBackwardPackets { get; set; }
        public float TotalLengthofFwdPackets { get; set; }
        public float TotalLengthofBwdPackets { get; set; }
        public float FwdPacketLengthMax { get; set; }
        public float FwdPacketLengthMin { get; set; }
        public float FwdPacketLengthMean { get; set; }
        public float FwdPacketLengthStd { get; set; }
    }

    // Model output class
    public class ModelOutput
    {
        public string PredictedLabel { get; set; }
        public float Score { get; set; }
    }
}
