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
            string modelPath = @"C:\Users\BoraErki\Desktop\CyberSecurityLogAnalyzer\MLNetRiskPredictor\RiskScoreModel3.zip";

            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"Model file not found at path: {modelPath}");

            DataViewSchema modelSchema;
            ITransformer trainedModel = _mlContext.Model.Load(modelPath, out modelSchema);

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);
        }

        public float PredictRiskScore(ModelInput input)
        {
            var prediction = _predictionEngine.Predict(input);
            return prediction.Probability;
        }
    }

    public class ModelInput
    {
         public float DestinationPort { get; set; }
         public float FlowDuration { get; set; }
         public float TotalFwdPackets { get; set; }
         public float TotalBackwardPackets { get; set; }
         public float TotalLengthFwdPackets { get; set; } // Düzeltildi
         public float TotalLengthBwdPackets { get; set; }
         public float FwdPacketLengthMax { get; set; }
         public float FwdPacketLengthMin { get; set; }
         public float FwdPacketLengthMean { get; set; }
         public float FwdPacketLengthStd { get; set; }
         public float BwdPacketLengthMax { get; set; }
         public float BwdPacketLengthMin { get; set; }
         public float BwdPacketLengthMean { get; set; }
         public float BwdPacketLengthStd { get; set; }
         public float FlowBytesPerSec { get; set; }
         public float FlowPacketsPerSec { get; set; }
         public float FlowIATMean { get; set; }
         public float FlowIATStd { get; set; }
         public float FlowIATMax { get; set; }
         public float FlowIATMin { get; set; }
         public float FwdIATTotal { get; set; }
         public float FwdIATMean { get; set; }
         public float FwdIATStd { get; set; }
         public float FwdIATMax { get; set; }
         public float FwdIATMin { get; set; }
         public float BwdIATTotal { get; set; }
         public float BwdIATMean { get; set; }
         public float BwdIATStd { get; set; }
         public float BwdIATMax { get; set; }
         public float BwdIATMin { get; set; }
         public float FwdPSHFlags { get; set; }
         public float BwdPSHFlags { get; set; }
         public float FwdURGFlags { get; set; }
         public float BwdURGFlags { get; set; }
         public float FwdHeaderLength { get; set; }
         public float BwdHeaderLength { get; set; }
         public float FwdPacketsPerSec { get; set; }
         public float BwdPacketsPerSec { get; set; }
         public float MinPacketLength { get; set; }
         public float MaxPacketLength { get; set; }
         public float PacketLengthMean { get; set; }
         public float PacketLengthStd { get; set; }
         public float PacketLengthVariance { get; set; }
         public float FINFlagCount { get; set; }
         public float SYNFlagCount { get; set; }
         public float RSTFlagCount { get; set; }
         public float PSHFlagCount { get; set; }
         public float ACKFlagCount { get; set; }
         public float URGFlagCount { get; set; }
         public float CWEFlagCount { get; set; }
         public float ECEFlagCount { get; set; }
         public float DownUpRatio { get; set; }
         public float AveragePacketSize { get; set; }
         public float AvgFwdSegmentSize { get; set; }
         public float AvgBwdSegmentSize { get; set; }
         public float FwdHeaderLength2 { get; set; }
         public float FwdAvgBytesBulk { get; set; }
         public float FwdAvgPacketsBulk { get; set; }
         public float FwdAvgBulkRate { get; set; }
         public float BwdAvgBytesBulk { get; set; }
         public float BwdAvgPacketsBulk { get; set; }
         public float BwdAvgBulkRate { get; set; }
         public float SubflowFwdPackets { get; set; }
         public float SubflowFwdBytes { get; set; }
         public float SubflowBwdPackets { get; set; }
         public float SubflowBwdBytes { get; set; }
         public float InitWinBytesForward { get; set; }
         public float InitWinBytesBackward { get; set; }
         public float ActDataPktFwd { get; set; }
         public float MinSegSizeForward { get; set; } // Düzeltildi
         public float ActiveMean { get; set; }
         public float ActiveStd { get; set; }
         public float ActiveMax { get; set; }
         public float ActiveMin { get; set; }
         public float IdleMean { get; set; }
         public float IdleStd { get; set; }
         public float IdleMax { get; set; }
         public float IdleMin { get; set; }
    }

    public class ModelOutput
    {
        public bool PredictedLabel { get; set; }
        public float Score { get; set; }
        public float Probability { get; set; }
    }
}
