using StoriesWeb.Models.Helpers;

namespace StoriesWeb.Services
{
  public interface IEmailService
  {
    Task<ApiResponse<string>> SendEmailAsync(string email, string subject, string body);

    Task<ApiResponse<object>> TestSettings(SmtpSettingsModel settings, string email);
  }
}