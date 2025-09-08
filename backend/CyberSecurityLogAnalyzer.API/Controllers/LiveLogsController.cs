using Microsoft.AspNetCore.Mvc;
using CyberSecurityLogAnalyzer.Core.Services;
using CyberSecurityLogAnalyzer.API.Data;
using CyberSecurityLogAnalyzer.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace CyberSecurityLogAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiveLogsController : ControllerBase
    {
        private readonly LogGeneratorService _logGeneratorService;
        private readonly LogDbContext _context;
        private readonly IOptionsMonitor<AdminSettings> _adminSettings; // IOptions yerine IOptionsMonitor kullanıyoruz

        public LiveLogsController(LogDbContext context, IOptionsMonitor<AdminSettings> adminSettings)
        {
            _logGeneratorService = new LogGeneratorService();
            _context = context;
            _adminSettings = adminSettings;
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs(int count = 10)
        {
            var liveLogs = _logGeneratorService.GenerateRandomLogs(count);
            
            var newLogs = liveLogs.Select(l => new LogModel
            {
                Timestamp = l.Timestamp,
                SourceIP = l.SourceIP,
                DestinationIP = l.DestinationIP,
                RiskScore = l.RiskScore,
                Action = l.Action,
                Status = l.Status,
                Server = l.Server
            }).ToList();

            _context.Logs.AddRange(newLogs);
            await _context.SaveChangesAsync();

            var logs = await _context.Logs
                .OrderByDescending(l => l.Timestamp)
                .Take(count)
                .ToListAsync();
                
            return Ok(logs);
        }

        [HttpGet("trends")]
        public async Task<IActionResult> GetTrends([FromQuery] int days = 7)
        {
            var startDate = DateTime.UtcNow.AddDays(-days);

            var logs = await _context.Logs
                .Where(l => l.Timestamp >= startDate)
                .OrderBy(l => l.Timestamp)
                .ToListAsync();

            return Ok(logs);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchLogs(
            [FromQuery] string? startDate = null,
            [FromQuery] string? endDate = null,
            [FromQuery] string? sourceIP = null,
            [FromQuery] string? destinationIP = null,
            [FromQuery] double? riskScore = null,
            [FromQuery] string? action = null)
        {
            var query = _context.Logs.AsQueryable();

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var start))
            {
                query = query.Where(l => l.Timestamp >= start);
            }
            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var end))
            {
                query = query.Where(l => l.Timestamp <= end.AddDays(1));
            }
            if (!string.IsNullOrEmpty(sourceIP))
            {
                query = query.Where(l => l.SourceIP.Contains(sourceIP));
            }
            if (riskScore.HasValue)
            {
                query = query.Where(l => l.RiskScore >= riskScore.Value);
            }
            if (!string.IsNullOrEmpty(action))
            {
                query = query.Where(l => l.Action.Contains(action));
            }

            var filteredLogs = await query
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();

            return Ok(filteredLogs);
        }
        
        public class LoginModel
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // O anki güncel şifre değerini alıyoruz
            var currentPassword = _adminSettings.CurrentValue.AdminPassword;

            if (model.Username == "admin" && model.Password == currentPassword)
            {
                return Ok("Giriş başarılı.");
            }
            else
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");
            }
        }
    }
}