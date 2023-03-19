using StoriesTranslationServices.Models;
using StoriesTranslationServices.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StoriesTranslationServices.Services
{
  public class TranslationsSettingsService : ITranslationsSettingsService
  {
    public string CurrentDirectory { get; set; }
    public string FilePath { get; set; }

    public TranslationsSettingsService()
    {
      CurrentDirectory = Environment.CurrentDirectory;
      FilePath = Path.Combine(CurrentDirectory, "setting.json");
    }

    public async Task<ApiResponse<Settings>> GetSettingsAsync()
    {
      ApiResponse<Settings> response = new();
      response.Data = new Settings();

      if (File.Exists(FilePath))
      {
        string FileText = await File.ReadAllTextAsync(FilePath);
        try
        {
          response.Data = JsonSerializer.Deserialize<Settings>(FileText);
          return response;
        }
        catch (Exception ex)
        {
          response.Success = false;
          response.Error = ex.Message;
          return response;
        }
      }
      response.Success = false;
      response.Error = "File not found";
      return response;
    }

    public async Task<ApiResponse<string>> SaveSettingsAsync(Settings settings)
    {
      ApiResponse<string> result = new();
      if (File.Exists(FilePath))
      {
        try
        {
          File.Delete(FilePath);
        }
        catch (Exception ex)
        {
          result.Success = false;
          result.Error = ex.Message;
          return result;
        }
      }
      try
      {
        string toStore = JsonSerializer.Serialize<Settings>(settings);
        await File.WriteAllTextAsync(FilePath, toStore, Encoding.UTF8);
        return result;
      }
      catch (Exception ex)
      {
        result.Success = false;
        result.Error = ex.Message;
        return result;
      }
    }

    public bool SettingsExist()
    {
      return File.Exists(FilePath);
    }
  }
}