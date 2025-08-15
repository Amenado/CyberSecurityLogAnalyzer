using CyberSecurityLogAnalyzer.Core.Services; // EKLENDİ
using Microsoft.EntityFrameworkCore;
using CyberSecurityLogAnalyzer.API.Data;
using CyberSecurityLogAnalyzer.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Risk score servisini ekle
builder.Services.AddSingleton<RiskScoreService>();

// DbContext ekleme
builder.Services.AddDbContext<LogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS ekle (frontend farklı porttan erişebilsin)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(); // CORS'u uygula
app.UseAuthorization();
app.MapControllers(); // Controller endpointlerini uygula

app.Run();
