using StoriesTranslationServices.Models;
using ToolsLibrary.Models;

namespace StoriesTranslationServices.Services
{
  public interface ITranslationsSettingsService
  {
    string CurrentDirectory { get; set; }
    string FilePath { get; set; }

    Task<ApiResponse<Settings>> GetSettingsAsync();

    Task<ApiResponse<string>> SaveSettingsAsync(Settings settings);

    bool SettingsExist();
  }
}