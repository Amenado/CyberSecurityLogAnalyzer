using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.IO;
using CyberSecurityLogAnalyzer.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

namespace CyberSecurityLogAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IOptionsMonitor<AdminSettings> _adminSettings;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public SettingsController(IOptionsMonitor<AdminSettings> adminSettings, IConfiguration configuration, IWebHostEnvironment env)
        {
            _adminSettings = adminSettings;
            _configuration = configuration;
            _env = env;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest("Yeni şifreler eşleşmiyor.");
            }

            // Mevcut şifre kontrolü
            if (model.CurrentPassword != _adminSettings.CurrentValue.AdminPassword)
            {
                return BadRequest("Mevcut şifre hatalı.");
            }

            try
            {
                var appSettingsPath = Path.Combine(_env.ContentRootPath, "appsettings.json");
                var json = System.IO.File.ReadAllText(appSettingsPath);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                jsonObj["AdminSettings"]["AdminPassword"] = model.NewPassword;
                string newJson = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                System.IO.File.WriteAllText(appSettingsPath, newJson);

                return Ok("Şifre başarıyla değiştirildi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Şifre değiştirilirken bir hata oluştu: {ex.Message}");
            }
        }
        [HttpPost("notifications")]
public async Task<IActionResult> UpdateNotifications([FromBody] NotificationsModel model)
{
    try
    {
        // Bildirim ayarlarını log dosyasına kaydetme
        // Bu kısım, veritabanına veya appsettings.json'a kaydetme olarak da değiştirilebilir.
        // Şimdilik basit bir loglama yapalım.
        string logPath = Path.Combine(_env.ContentRootPath, "notifications.log");
        string notificationStatus = $"E-posta Bildirimleri: {model.EmailNotifications}, SMS Bildirimleri: {model.SmsNotifications} - {DateTime.Now}";
        await System.IO.File.AppendAllTextAsync(logPath, notificationStatus + Environment.NewLine);

        return Ok(new { message = "Bildirim ayarları başarıyla güncellendi." });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "Bildirim ayarları kaydedilirken bir hata oluştu.", error = ex.Message });
    }
}

public class NotificationsModel
{
    public bool EmailNotifications { get; set; }
    public bool SmsNotifications { get; set; }
}
    }
}