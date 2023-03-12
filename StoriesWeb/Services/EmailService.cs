using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
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

    public EmailService(ApplicationDbContext context, ILogger<EmailService> logger)
    {
      _context = context;
      _logger = logger;
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
      message.Subject = "Test SMTP";
      message.Body = new TextPart("html")
      { Text = "Testing message" };
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
        }
      }
      if (result.Success)
      {
        try
        {
          await client.SendAsync(message);
          await client.DisconnectAsync(true);
          result.SendEmailTest = TestResults.Passed;
        }
        catch (Exception ex)
        {
          recepient.Sent = false;
          recepient.Error = ex.Message;
          recepient.SentAt = DateTime.UtcNow;
          await _context.EmailRecepients.AddAsync(recepient);
          await _context.SaveChangesAsync();
          result.SendEmailTest = TestResults.Failed;
          result.Error = ex.Message;
          result.Success = false;
        }
        recepient.Sent = true;
        recepient.SentAt = DateTime.UtcNow;
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

    private class SmtpTestResponse
    {
      public TestResults ConnectionTest { get; set; } = TestResults.Missed;
      public TestResults LoginTest { get; set; } = TestResults.Missed;
      public TestResults SendEmailTest { get; set; } = TestResults.Missed;
      public string Error = string.Empty;
      public bool Success = true;
    }
  }
}