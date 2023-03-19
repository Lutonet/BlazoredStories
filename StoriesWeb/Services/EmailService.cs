using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MimeKit;
using StoriesWeb.Data;
using StoriesWeb.Models;
using StoriesWeb.Models.Helpers;

namespace StoriesWeb.Services
{
  public class EmailService : IEmailService
  {
    private readonly ApplicationDbContext _context;
    private ILogger<EmailService> _logger;
    private IStringLocalizer<EmailService> t;

    public EmailService(ApplicationDbContext context,
                        ILogger<EmailService> logger,
                        IStringLocalizer<EmailService> _t)
    {
      _context = context;
      _logger = logger;
      t = _t;
    }

    public async Task<ApiResponse<object>> TestSettings(SmtpSettingsModel settings, string email)
    {
      EmailLog emaillog = new();
      EmailRecepient recepient = new();

      emaillog.Subject = "Test SMTP";
      await _context.EmailLogs.AddAsync(emaillog);
      await _context.SaveChangesAsync();
      recepient.EmailLogId = emaillog.Id;

      _logger.LogInformation("Testing SMTP settings", new SmtpSettingsModel
      {
        SmtpServer = settings.SmtpServer,
        SmtpPort = settings.SmtpPort,
        SmtpUser = settings.SmtpUser,
        SmtpPassword = "********",
        UseAuthentication = settings.UseAuthentication,
        SecureSocketOptions = settings.SecureSocketOptions,
      });
      SmtpTestResponse result = new SmtpTestResponse();
      MimeMessage message = new();
      message.From.Add(new MailboxAddress("", "stories@outlook.cz"));
      message.To.Add(new MailboxAddress("", email));
      message.Subject = "Testing SMTP settings";
      message.Body = new TextPart("html")
      { Text = "Testing SMTP settings" };
      using SmtpClient client = new();
      try
      {
        await client.ConnectAsync(settings.SmtpServer, settings.SmtpPort, settings.SecureSocketOptions);
        result.ConnectionTest = TestResults.Passed;
      }
      catch (Exception ex)
      {
        recepient.Sent = false;
        recepient.Error = ex.Message;
        recepient.SentAt = DateTime.UtcNow;
        await _context.EmailRecepients.AddAsync(recepient);
        await _context.SaveChangesAsync();
        result.ConnectionTest = TestResults.Failed;
        result.Error = ex.Message;
        result.Success = false;
        return new ApiResponse<Object>()
        {
          Successful = false,
          ErrorMessage = recepient.Error
        };
      }
      if (result.Success)
      {
        try
        {
          await client.AuthenticateAsync(settings.SmtpUser, settings.SmtpPassword);
          result.LoginTest = TestResults.Passed;
        }
        catch (Exception ex)
        {
          recepient.Sent = false;
          recepient.Error = ex.Message;
          recepient.SentAt = DateTime.UtcNow;
          await _context.EmailRecepients.AddAsync(recepient);
          await _context.SaveChangesAsync();
          result.LoginTest = TestResults.Failed;
          result.Error = ex.Message;
          result.Success = false;
          return new ApiResponse<Object>()
          {
            Successful = false,
            ErrorMessage = recepient.Error
          };
        }
      }
      if (result.Success)
      {
        try
        {
          await client.SendAsync(message);
          await client.DisconnectAsync(true);
          recepient.Sent = true;
          recepient.SentAt = DateTime.UtcNow;
          result.SendEmailTest = TestResults.Passed;
        }
        catch (Exception ex)
        {
          recepient.Sent = false;
          recepient.Error = ex.Message;
          recepient.SentAt = DateTime.UtcNow;

          result.SendEmailTest = TestResults.Failed;
          result.Error = ex.Message;
          result.Success = false;
        }

        await _context.EmailRecepients.AddAsync(recepient);
        await _context.SaveChangesAsync();
        return new ApiResponse<object>()
        {
          Data = result,
          Successful = result.Success,
          ErrorMessage = result.Error
        };
      }
      return new ApiResponse<object>()
      {
        Successful = false,
        ErrorMessage = result.Error
      };
    }

    public async Task<ApiResponse<string>> SendEmailAsync(string email, string subject, string body)
    {
      ApiResponse<string> response = new();
      EmailLog emaillog = new();
      EmailRecepient recepient = new();
      emaillog.Subject = subject;
      await _context.EmailLogs.AddAsync(emaillog);
      await _context.SaveChangesAsync();
      recepient.EmailLogId = emaillog.Id;

      ApplicationSetup settings = await _context.ApplicationSetups.FirstOrDefaultAsync();
      if (settings == null)
      {
        recepient.Error = "Email settings were not found";
        recepient.Sent = false;
        await _context.EmailRecepients.AddAsync(recepient);
        await _context.SaveChangesAsync();
        return new ApiResponse<string>() { Data = null, Successful = false, ErrorMessage="Smtp settings not found" };
      }

      MimeMessage message = new();
      message.From.Add(new MailboxAddress("", settings.EmailFrom));
      message.To.Add(new MailboxAddress("", email));
      message.Subject = subject;
      message.Body = new TextPart("html")
      { Text = body };
      using SmtpClient client = new SmtpClient();
      try
      {
        await client.ConnectAsync(settings.SmtpServer, settings.SmtpPort, settings.SecureSocketOptions);
      }
      catch (Exception ex)
      {
        recepient.Sent = false;
        recepient.Error = ex.Message;
        await _context.EmailRecepients.AddAsync(recepient);
        await _context.SaveChangesAsync();
        response.ErrorMessage = ex.Message;
        response.Successful = false;
        return response;
      }
      try
      {
        await client.AuthenticateAsync(settings.SmtpUser, settings.SmtpPassword);
      }
      catch (Exception ex)
      {
        recepient.Sent = false;
        recepient.Error = ex.Message;
        await _context.EmailRecepients.AddAsync(recepient);
        await _context.SaveChangesAsync();
        response.ErrorMessage = ex.Message;
        response.Successful = false;
        return response;
      }
      try
      {
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
      }
      catch (Exception ex)
      {
        recepient.Sent = false;
        recepient.Error = ex.Message;
        await _context.EmailRecepients.AddAsync(recepient);
        await _context.SaveChangesAsync();
        response.ErrorMessage = ex.Message;
        response.Successful = false;
        return response;
      }
      recepient.Sent = true;
      recepient.SentAt = DateTime.UtcNow;
      await _context.EmailRecepients.AddAsync(recepient);
      await _context.SaveChangesAsync();
      return new ApiResponse<string>()
      {
        Data = "Email sent successfuly"
      };
    }

    public async Task<ApiResponse<string>> SendCodeAsync(string callbackUrl, string email)
    {
      ApiResponse<string> result = new();
      string subj = t["Activation of the account"];
      result = await SendEmailAsync(email, subj, GenerateActivationEmail(callbackUrl));
      return result;
    }

    public async Task<ApiResponse<string>> SendResetPasswordCodeAsync(string callbackUrl, string email)
    {
      ApiResponse<string> result = new();
      string subj = t["Reset the password"];
      result = await SendEmailAsync(email, subj, GeneratePasswordResetEmail(callbackUrl));
      return result;
    }

    private class SmtpTestResponse
    {
      public TestResults ConnectionTest { get; set; } = TestResults.Missed;
      public TestResults LoginTest { get; set; } = TestResults.Missed;
      public TestResults SendEmailTest { get; set; } = TestResults.Missed;
      public string Error = string.Empty;
      public bool Success = true;
    }

    private string GenerateActivationEmail(string callbackUrl)
    {
      string emailBody = $"<h5 style=\"text-align: center\">{t["Hello"]}, </h5>" +
        $"<div> {t["Please"]}<a href=\"{callbackUrl}\"> {t["Click here"]}</a> {t["to confirm your account"]}";

      return emailBody;
    }

    private string GeneratePasswordResetEmail(string callbackUrl)
    {
      string emailBody = $"<h5 style=\"text-align: center\">{t["Hello"]}, </h5>" +
        $"<div> {t["Please"]}<a href=\"{callbackUrl}\"> {t["Click here"]}</a> {t["to reset your password"]}";

      return emailBody;
    }
  }
}