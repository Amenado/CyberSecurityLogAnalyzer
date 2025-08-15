using Microsoft.ML;
using Microsoft.ML.Data;

namespace CyberSecurityLogAnalyzer.Core
{
    public class LogInput
    {
        public float Feature1 { get; set; }
        public float Feature2 { get; set; }
        // ... logtan türeteceğin diğer sayısal özellikler
    }

    public class LogPrediction
    {
        [ColumnName("Score")]
        public float RiskScore { get; set; }
    }

    public class LogAnalyzer
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public LogAnalyzer()
        {
            _mlContext = new MLContext();

            // Basit örnek: One-Class SVM veya AnomalyDetectionTrainer kullanılabilir
            var dataView = _mlContext.Data.LoadFromEnumerable(new List<LogInput>());
            var pipeline = _mlContext.Transforms.NormalizeMinMax("Feature1")
                .Append(_mlContext.AnomalyDetection.Trainers.RandomizedPca("Feature1", rank: 2));

            _model = pipeline.Fit(dataView);
        }

        public float PredictRisk(LogInput log)
        {
            var predEngine = _mlContext.Model.CreatePredictionEngine<LogInput, LogPrediction>(_model);
            var prediction = predEngine.Predict(log);
            return prediction.RiskScore;
        }
    }
}
