using StoriesWeb.Models.Helpers;
using ToolsLibrary.Models;

namespace StoriesWeb.Services
{
  public interface IEmailService
  {
    Task<ApiResponse<string>> SendCodeAsync(string callbackUrl, string email);

    Task<ApiResponse<string>> SendEmailAsync(string email, string subject, string body);

    Task<ApiResponse<string>> SendResetPasswordCodeAsync(string callbackUrl, string email);

    Task<ApiResponse<object>> TestSettings(SmtpSettingsModel settings, string email);
  }
}