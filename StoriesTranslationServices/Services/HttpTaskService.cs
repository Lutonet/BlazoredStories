using StoriesTranslationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace StoriesTranslationServices.Services
{
  public class HttpTaskService : IHttpTaskService
  {
    private readonly ILibreApiService _api;
    private readonly ITranslationsSettingsService _settings;

    public HttpTaskService(ILibreApiService api, ITranslationsSettingsService settings)
    {
      _api = api;
      _settings = settings;
    }

    public async Task<List<LibreTranslateLanguage>> GetAllAvailableLanguagesAsync()
    {
      List<LibreTranslateLanguage> availableLanguages = new();
      ConcurrentBag<ServerLanguages> allLanguages = new ConcurrentBag<ServerLanguages>();
      var settings = await _settings.GetSettingsAsync();
      var servers = settings.Data.Servers;
      if (settings != null)
      {
        Parallel.ForEach(servers, async (server) =>
        {
          ServerLanguages serverLanguages = new ServerLanguages();

          HttpClient client = new HttpClient();
          client.BaseAddress = new Uri(server.ServerAddress+":"+server.ServerPort+"/languages");
          client.DefaultRequestHeaders.Add("ContentType", "application/json");

          serverLanguages.Server = server;
          try
          {
            serverLanguages.Languages = await client.GetFromJsonAsync<List<LibreTranslateLanguage>>(client.BaseAddress);
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
          if (serverLanguages.Languages != null)
          {
            allLanguages.Add(serverLanguages);
          }
        });
        foreach (var language in allLanguages)
        {
          var languagesAvailable = language.Languages;
          foreach (var lang in languagesAvailable)
          {
            if (!availableLanguages.Contains(lang))
            {
              availableLanguages.Add(lang);
            }
          }
        }
        return availableLanguages;
      }
      return new List<LibreTranslateLanguage>();
    }
  }
}