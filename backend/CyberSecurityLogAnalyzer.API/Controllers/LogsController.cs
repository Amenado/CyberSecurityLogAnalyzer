using Microsoft.AspNetCore.Mvc;
using CyberSecurityLogAnalyzer.API.Data;
using CyberSecurityLogAnalyzer.API.Models;

namespace CyberSecurityLogAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly LogDbContext _context;

        public LogsController(LogDbContext context)
        {
            _context = context;
        }

        // Tüm logları çek
        [HttpGet]
        public IActionResult GetAll()
        {
            var logs = _context.Logs.ToList();
            return Ok(logs);
        }

        // Random log ekle
        [HttpPost("random")]
        public IActionResult AddRandom()
        {
            var random = new Random();
            var log = new LogModel
            {
                Timestamp = DateTime.Now,
                SourceIP = $"192.168.1.{random.Next(1, 255)}",
                DestinationIP = $"10.0.0.{random.Next(1, 255)}",
                Action = random.Next(0, 2) == 0 ? "Login" : "FileUpload",
                Status = random.Next(0, 2) == 0 ? "Success" : "Failed",
                RiskScore = (float)Math.Round(random.NextDouble(), 2),
                Server = $"DemoServer{random.Next(1, 5)}",
                User = $"User{random.Next(1, 10)}"
            };

            _context.Logs.Add(log);
            _context.SaveChanges();

            return Ok(log);
        }
    }
}
