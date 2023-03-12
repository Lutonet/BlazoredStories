using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MudBlazor.Services;
using Serilog;
using StoriesI18n.Middlewares;
using StoriesI18n.Models;
using StoriesI18n.Services;
using StoriesWeb.Areas.Identity;
using StoriesWeb.Data;
using StoriesWeb.Hubs;
using StoriesWeb.Models;
using StoriesWeb.Services;

namespace StoriesWeb
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
    .WriteTo.SQLite(@"log.db")
    .CreateLogger();

      var builder = WebApplication.CreateBuilder(args);
      builder.Host.UseSerilog();
      I18nConfigurationModel I18nSettings = await new I18nSettingsController().Load();
      // Add services to the container.
      var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
      builder.Services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlite(connectionString));

      builder.Services.AddDatabaseDeveloperPageExceptionFilter();
      builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
      {
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        options.SignIn.RequireConfirmedAccount = false;

        options.User.RequireUniqueEmail = true;
      }).AddEntityFrameworkStores<ApplicationDbContext>();
      builder.Services.AddLocalization();
      builder.Services.AddMudServices();
      builder.Services.AddAuthentication();
      builder.Services.AddSingleton(I18nSettings);
      builder.Services.AddTransient<IFirstRunService, FirstRunService>();
      builder.Services.AddTransient<IEmailService, EmailService>();
      builder.Services.AddTransient<I18nMiddleware>();
      builder.Services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();
      builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<UserModel>>();
      builder.Services.AddRazorPages();
      builder.Services.AddServerSideBlazor();
      builder.Services.AddResponseCompression(opts =>
      {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                  new[] { "application/octet-stream" });
      });

      var app = builder.Build();
      app.UseResponseCompression();
      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseMigrationsEndPoint();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios,
        // see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseMiddleware<I18nMiddleware>();
      app.MapControllers();
      app.MapRazorPages();
      app.UseHttpsRedirection();
      app.UseRequestLocalization();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthorization();

      app.MapBlazorHub();
      app.MapHub<IndexHub>("/indexhub");

      app.MapFallbackToPage("/_Host");

      app.Run();
    }
  }
}