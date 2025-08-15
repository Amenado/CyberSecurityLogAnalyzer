using Microsoft.ML.Data;

namespace CyberSecurityLogAnalyzer.Core.Models
{
    public class RiskPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }

        public float Score { get; set; }
    }
}
