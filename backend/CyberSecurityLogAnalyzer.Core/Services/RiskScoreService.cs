using Microsoft.ML;
using System.IO;

namespace CyberSecurityLogAnalyzer.Core.Services
{
    public class RiskScoreService
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<ModelInput, ModelOutput> _predictionEngine;

        public RiskScoreService()
        {
            _mlContext = new MLContext();
            string modelPath = @"C:\Users\BoraErki\Desktop\CyberSecurityLogAnalyzer\MLNetRiskPredictor\RiskScoreModel.zip";

            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"Model dosyası bulunamadı: {modelPath}");

            DataViewSchema modelSchema;
            var trainedModel = _mlContext.Model.Load(modelPath, out modelSchema);

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);
        }

        public float PredictRiskScore(ModelInput input)
        {
            var prediction = _predictionEngine.Predict(input);
            return prediction.Probability;
        }
    }

}
