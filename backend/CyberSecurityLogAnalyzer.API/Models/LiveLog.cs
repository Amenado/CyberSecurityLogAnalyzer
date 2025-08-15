using System;

namespace CyberSecurityLogAnalyzer.Core.Models
{
    public class LiveLog
    {
        public DateTime Timestamp { get; set; }
        public string SourceIP { get; set; }
        public string DestinationIP { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public double RiskScore { get; set; }
        public string Server { get; set; }
    }
}
