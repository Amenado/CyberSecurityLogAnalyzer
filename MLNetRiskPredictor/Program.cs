using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Linq;

namespace MLNetRiskPredictor
{
    public class NetworkLog
    {
        [LoadColumn(0)] public float DestinationPort { get; set; }
        [LoadColumn(1)] public float FlowDuration { get; set; }
        [LoadColumn(2)] public float TotalFwdPackets { get; set; }
        [LoadColumn(3)] public float TotalBackwardPackets { get; set; }
        [LoadColumn(4)] public float TotalLengthFwdPackets { get; set; } // Düzeltildi
        [LoadColumn(5)] public float TotalLengthBwdPackets { get; set; }
        [LoadColumn(6)] public float FwdPacketLengthMax { get; set; }
        [LoadColumn(7)] public float FwdPacketLengthMin { get; set; }
        [LoadColumn(8)] public float FwdPacketLengthMean { get; set; }
        [LoadColumn(9)] public float FwdPacketLengthStd { get; set; }
        [LoadColumn(10)] public float BwdPacketLengthMax { get; set; }
        [LoadColumn(11)] public float BwdPacketLengthMin { get; set; }
        [LoadColumn(12)] public float BwdPacketLengthMean { get; set; }
        [LoadColumn(13)] public float BwdPacketLengthStd { get; set; }
        [LoadColumn(14)] public float FlowBytesPerSec { get; set; }
        [LoadColumn(15)] public float FlowPacketsPerSec { get; set; }
        [LoadColumn(16)] public float FlowIATMean { get; set; }
        [LoadColumn(17)] public float FlowIATStd { get; set; }
        [LoadColumn(18)] public float FlowIATMax { get; set; }
        [LoadColumn(19)] public float FlowIATMin { get; set; }
        [LoadColumn(20)] public float FwdIATTotal { get; set; }
        [LoadColumn(21)] public float FwdIATMean { get; set; }
        [LoadColumn(22)] public float FwdIATStd { get; set; }
        [LoadColumn(23)] public float FwdIATMax { get; set; }
        [LoadColumn(24)] public float FwdIATMin { get; set; }
        [LoadColumn(25)] public float BwdIATTotal { get; set; }
        [LoadColumn(26)] public float BwdIATMean { get; set; }
        [LoadColumn(27)] public float BwdIATStd { get; set; }
        [LoadColumn(28)] public float BwdIATMax { get; set; }
        [LoadColumn(29)] public float BwdIATMin { get; set; }
        [LoadColumn(30)] public float FwdPSHFlags { get; set; }
        [LoadColumn(31)] public float BwdPSHFlags { get; set; }
        [LoadColumn(32)] public float FwdURGFlags { get; set; }
        [LoadColumn(33)] public float BwdURGFlags { get; set; }
        [LoadColumn(34)] public float FwdHeaderLength { get; set; }
        [LoadColumn(35)] public float BwdHeaderLength { get; set; }
        [LoadColumn(36)] public float FwdPacketsPerSec { get; set; }
        [LoadColumn(37)] public float BwdPacketsPerSec { get; set; }
        [LoadColumn(38)] public float MinPacketLength { get; set; }
        [LoadColumn(39)] public float MaxPacketLength { get; set; }
        [LoadColumn(40)] public float PacketLengthMean { get; set; }
        [LoadColumn(41)] public float PacketLengthStd { get; set; }
        [LoadColumn(42)] public float PacketLengthVariance { get; set; }
        [LoadColumn(43)] public float FINFlagCount { get; set; }
        [LoadColumn(44)] public float SYNFlagCount { get; set; }
        [LoadColumn(45)] public float RSTFlagCount { get; set; }
        [LoadColumn(46)] public float PSHFlagCount { get; set; }
        [LoadColumn(47)] public float ACKFlagCount { get; set; }
        [LoadColumn(48)] public float URGFlagCount { get; set; }
        [LoadColumn(49)] public float CWEFlagCount { get; set; }
        [LoadColumn(50)] public float ECEFlagCount { get; set; }
        [LoadColumn(51)] public float DownUpRatio { get; set; }
        [LoadColumn(52)] public float AveragePacketSize { get; set; }
        [LoadColumn(53)] public float AvgFwdSegmentSize { get; set; }
        [LoadColumn(54)] public float AvgBwdSegmentSize { get; set; }
        [LoadColumn(55)] public float FwdHeaderLength2 { get; set; }
        [LoadColumn(56)] public float FwdAvgBytesBulk { get; set; }
        [LoadColumn(57)] public float FwdAvgPacketsBulk { get; set; }
        [LoadColumn(58)] public float FwdAvgBulkRate { get; set; }
        [LoadColumn(59)] public float BwdAvgBytesBulk { get; set; }
        [LoadColumn(60)] public float BwdAvgPacketsBulk { get; set; }
        [LoadColumn(61)] public float BwdAvgBulkRate { get; set; }
        [LoadColumn(62)] public float SubflowFwdPackets { get; set; }
        [LoadColumn(63)] public float SubflowFwdBytes { get; set; }
        [LoadColumn(64)] public float SubflowBwdPackets { get; set; }
        [LoadColumn(65)] public float SubflowBwdBytes { get; set; }
        [LoadColumn(66)] public float InitWinBytesForward { get; set; }
        [LoadColumn(67)] public float InitWinBytesBackward { get; set; }
        [LoadColumn(68)] public float ActDataPktFwd { get; set; }
        [LoadColumn(69)] public float MinSegSizeForward { get; set; } // Düzeltildi
        [LoadColumn(70)] public float ActiveMean { get; set; }
        [LoadColumn(71)] public float ActiveStd { get; set; }
        [LoadColumn(72)] public float ActiveMax { get; set; }
        [LoadColumn(73)] public float ActiveMin { get; set; }
        [LoadColumn(74)] public float IdleMean { get; set; }
        [LoadColumn(75)] public float IdleStd { get; set; }
        [LoadColumn(76)] public float IdleMax { get; set; }
        [LoadColumn(77)] public float IdleMin { get; set; }
        [LoadColumn(78)] public bool Label { get; set; }
    }

    public class NetworkLogPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext(seed: 1);

            string dataPath = @"C:\Users\BoraErki\Desktop\CyberSecurityLogAnalyzer\MLNetRiskPredictor\data\benign_ddos.csv";
            
            var data = mlContext.Data.LoadFromTextFile<NetworkLog>(
                dataPath, 
                separatorChar: ',', 
                hasHeader: true,
                allowQuoting: false);

            string[] featureColumns = typeof(NetworkLog)
                .GetProperties()
                .Where(p => p.Name != "Label" &&
                            p.Name != "BwdPSHFlags" &&
                            p.Name != "FwdURGFlags" &&
                            p.Name != "BwdURGFlags" &&
                            p.Name != "CWE Flag Count" &&
                            p.Name != "FwdAvgBytesBulk" &&
                            p.Name != "FwdAvgPacketsBulk" &&
                            p.Name != "FwdAvgBulkRate" &&
                            p.Name != "BwdAvgBytesBulk" &&
                            p.Name != "BwdAvgPacketsBulk" &&
                            p.Name != "BwdAvgBulkRate")
                .Select(p => p.Name)
                .ToArray();

            var pipeline = mlContext.Transforms.Concatenate("Features", featureColumns)
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.BinaryClassification.Trainers.LightGbm(
                    labelColumnName: "Label",
                    featureColumnName: "Features"));

            // K = 5
            Console.WriteLine("K-Katlı Çapraz Doğrulama Başlatılıyor...");
            var crossValidationResults = mlContext.BinaryClassification.CrossValidate(
                data,
                pipeline,
                numberOfFolds: 5,
                labelColumnName: "Label");

            Console.WriteLine("\nK-Katlı Çapraz Doğrulama Sonuçları");
            Console.WriteLine("----------------------------------");

            int foldNumber = 1;
            foreach (var fold in crossValidationResults)
            {
                var metrics = fold.Metrics;
                Console.WriteLine($"\nKat {foldNumber} Performans Metrikleri:");
                Console.WriteLine($"  Doğruluk (Accuracy): {metrics.Accuracy:P2}");
                Console.WriteLine($"  F1 Score: {metrics.F1Score:P2}");
                Console.WriteLine($"  AUC: {metrics.AreaUnderRocCurve:P2}");
                foldNumber++;
            }

            var averageAccuracy = crossValidationResults.Average(fold => fold.Metrics.Accuracy);
            var averageF1Score = crossValidationResults.Average(fold => fold.Metrics.F1Score);
            var averageAuc = crossValidationResults.Average(fold => fold.Metrics.AreaUnderRocCurve);

            Console.WriteLine("\nOrtalama Performans Metrikleri:");
            Console.WriteLine($"  Ortalama Doğruluk (Accuracy): {averageAccuracy:P2}");
            Console.WriteLine($"  Ortalama F1 Score: {averageF1Score:P2}");
            Console.WriteLine($"  Ortalama AUC: {averageAuc:P2}");

            var dataEnumerable = mlContext.Data.CreateEnumerable<NetworkLog>(data, reuseRowObject: true);
            var benignCount = dataEnumerable.Count(x => x.Label == false);
            var ddosCount = dataEnumerable.Count(x => x.Label == true);
            Console.WriteLine($"\nBenign: {benignCount}, DDoS: {ddosCount}");

            var finalTrainedModel = pipeline.Fit(data);
            mlContext.Model.Save(finalTrainedModel, data.Schema, "RiskScoreModel3.zip");
            Console.WriteLine("\nFinal model tüm veri üzerinde eğitilerek kaydedildi.");

            var predEngine = mlContext.Model.CreatePredictionEngine<NetworkLog, NetworkLogPrediction>(finalTrainedModel);
            var sample = mlContext.Data.CreateEnumerable<NetworkLog>(data, reuseRowObject: false).First();
            var prediction = predEngine.Predict(sample);

            Console.WriteLine($"\nÖrnek Tahmin:");
            Console.WriteLine($"  Tahmin: {prediction.PredictedLabel}");
            Console.WriteLine($"  Risk Skoru: {prediction.Probability:0.00}");
        }
    }
}