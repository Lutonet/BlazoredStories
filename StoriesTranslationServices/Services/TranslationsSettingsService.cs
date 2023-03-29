using StoriesTranslationServices.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToolsLibrary.Models;

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
          response.Successful = false;
          response.ErrorMessage = ex.Message;
          return response;
        }
      }
      response.Successful = false;
      response.ErrorMessage = "File not found";
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
          result.Successful = false;
          result.ErrorMessage = ex.Message;
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
        result.Successful = false;
        result.ErrorMessage = ex.Message;
        return result;
      }
    }

    public bool SettingsExist()
    {
      return File.Exists(FilePath);
    }
  }
}