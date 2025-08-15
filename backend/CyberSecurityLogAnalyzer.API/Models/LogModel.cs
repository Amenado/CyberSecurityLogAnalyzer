using System;

namespace CyberSecurityLogAnalyzer.API.Models
{
    public class LogModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? SourceIP { get; set; }
        public string? DestinationIP { get; set; }
        public string? Protocol { get; set; }
        public string? User { get; set; }
        public string? Action { get; set; }
        public string? Status { get; set; }
        public string? Server { get; set; }
        public float RiskScore { get; set; } // ML.NET ile hesaplanacak
    }
}
