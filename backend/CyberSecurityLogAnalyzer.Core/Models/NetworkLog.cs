using Microsoft.ML.Data;

namespace CyberSecurityLogAnalyzer.Core.Models
{
    public class NetworkLog
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

        [LoadColumn(78)]
        public string Label { get; set; } // BENIGN / ANOMALY
    }
}
