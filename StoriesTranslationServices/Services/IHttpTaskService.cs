using StoriesTranslationServices.Models;

namespace StoriesTranslationServices.Services
{
  public interface IHttpTaskService
  {
    Task<List<LibreTranslateLanguage>> GetAllAvailableLanguagesAsync();
  }
}