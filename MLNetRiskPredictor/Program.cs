using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Linq;

namespace MLNetRiskPredictor
{
    // CSV'deki veri yapısı
    public class NetworkLog
    {
        [LoadColumn(0)] public float DestinationPort { get; set; }
        [LoadColumn(1)] public float FlowDuration { get; set; }
        [LoadColumn(2)] public float TotalFwdPackets { get; set; }
        [LoadColumn(3)] public float TotalBackwardPackets { get; set; }
        [LoadColumn(4)] public float TotalLengthFwdPackets { get; set; }
        [LoadColumn(5)] public float TotalLengthBwdPackets { get; set; }
        [LoadColumn(6)] public float FwdPacketLengthMax { get; set; }
        [LoadColumn(7)] public float FwdPacketLengthMin { get; set; }
        [LoadColumn(8)] public float FwdPacketLengthMean { get; set; }
        [LoadColumn(9)] public float FwdPacketLengthStd { get; set; }
        [LoadColumn(78)] public string Label { get; set; } = string.Empty;
    }

    // Tahmin sonucu
    public class NetworkLogPrediction
    {
        [ColumnName("PredictedLabel")] public string PredictedLabel { get; set; } = string.Empty;
        [ColumnName("Score")] public float[] Score { get; set; } = Array.Empty<float>(); 
    }

    class Program
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            // CSV dosyasının path'i
            string dataPath = @"C:\Users\BoraErki\Desktop\CyberSecurityLogAnalyzer\MLNetRiskPredictor\data\Friday-WorkingHours-Afternoon-DDos.pcap_ISCX.csv";

            // Veriyi yükle
            var data = mlContext.Data.LoadFromTextFile<NetworkLog>(
                path: dataPath,
                separatorChar: ',',
                hasHeader: true);

            // Modeli yükle
            ITransformer trainedModel = mlContext.Model.Load(
                "RiskScoreModel.zip",
                out var modelInputSchema);

            var predEngine = mlContext.Model.CreatePredictionEngine<NetworkLog, NetworkLogPrediction>(trainedModel);

            // Veriyi enumerate et ve tahminleri göster
            var dataEnumerable = mlContext.Data.CreateEnumerable<NetworkLog>(data, reuseRowObject: false);

            Console.WriteLine("Destination Port | Flow Duration | ... | Predicted Risk | Risk Score");
            foreach (var row in dataEnumerable)
            {
                var prediction = predEngine.Predict(row);
                Console.WriteLine($"{row.DestinationPort} | {row.FlowDuration} | ... | {prediction.PredictedLabel} | {prediction.Score[0]:0.00}");
            }

            Console.WriteLine("\nTahminler tamamlandı.");
        }
    }
}
