using StoriesTranslationServices.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToolsLibrary.Models;

namespace StoriesTranslationServices.Services
{
  public class LibreApiService : ILibreApiService
  {
    private List<LibreTranslateLanguage> _languages;

    public async Task<ApiResponse<string>> TestConnectionAsync(LibreTranslateServer server)
    {
      try
      {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(server.ServerAddress+":"+server.ServerPort+"/languages");
        client.DefaultRequestHeaders.Add("ContentType", "application/json");

        _languages = await client.GetFromJsonAsync<List<LibreTranslateLanguage>>(client.BaseAddress);
        return new ApiResponse<string>()
        {
          Successful = true,
          Data = _languages.Count.ToString()
        };
      }
      catch (Exception ex)
      {
        return new ApiResponse<string>()
        {
          Successful = false,
          ErrorMessage = ex.Message
        };
      }
    }

    public async Task<ApiResponse<List<LibreTranslateLanguage>>> GetListOfLanguagesAsync(LibreTranslateServer server)
    {
      ApiResponse<List<LibreTranslateLanguage>> response = new();
      HttpClient client = new HttpClient();
      try
      {
        client.BaseAddress = new Uri(server.ServerAddress+":"+server.ServerPort+"/languages");
        client.DefaultRequestHeaders.Add("ContentType", "application/json");
      }
      catch (Exception ex)
      {
        response.Successful = false;
        response.ErrorMessage = ex.Message;
        return response;
      }
      try
      {
        response.Data = await client.GetFromJsonAsync<List<LibreTranslateLanguage>>(client.BaseAddress);
        return response;
      }
      catch (Exception ex)
      {
        response.Successful = false;
        response.ErrorMessage = ex.Message;
        return response;
      }
    }

    public async Task<ApiResponse<string>> TranslateAsync(TranslateRequest request, LibreTranslateServer server)
    {
      ApiResponse<string> response = new();

      HttpClient client = new HttpClient();
      client.BaseAddress = new Uri(server.ServerAddress+":"+server.ServerPort + "/translate");
      client.DefaultRequestHeaders.Add("ContentType", "application/json");

      LibreRequest req = new LibreRequest();
      req.source = request.SourceLanguageCode;
      req.target = request.TargetLanguageCode;
      req.q = request.TextToTranslate;
      req.format = "text";

      string json = JsonSerializer.Serialize<LibreRequest>(req);
      var data = new StringContent(json, Encoding.UTF8, "application/json");
      try
      {
        var answer = await client.PostAsync(client.BaseAddress, data);
        var result = await answer.Content.ReadAsStringAsync();
        LibreResponse libre = JsonSerializer.Deserialize<LibreResponse>(result);
        response.Successful = true;
        response.Data = libre.translatedText;
        return response;
      }
      catch
      (Exception ex)
      {
        response.Successful= false;
        response.ErrorMessage = ex.Message;
        return response;
      }
    }

    private class LibreRequest
    {
      public string q { get; set; } = string.Empty;
      public string source { get; set; } = string.Empty;
      public string target { get; set; } = string.Empty;
      public string format { get; set; } = string.Empty;
      public string apiKey { get; set; } = string.Empty;
    }

    public class LibreResponse
    {
      public string translatedText { get; set; }
    }
  }
}