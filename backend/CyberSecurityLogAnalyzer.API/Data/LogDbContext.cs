using Microsoft.EntityFrameworkCore;
using CyberSecurityLogAnalyzer.API.Models;

namespace CyberSecurityLogAnalyzer.API.Data
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

        public DbSet<LogModel> Logs { get; set; }
    }
}

