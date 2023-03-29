using StoriesTranslationServices.Models;
using ToolsLibrary.Models;

namespace StoriesTranslationServices.Services
{
  public interface ILibreApiService
  {
    Task<ApiResponse<List<LibreTranslateLanguage>>> GetListOfLanguagesAsync(LibreTranslateServer server);

    Task<ApiResponse<string>> TestConnectionAsync(LibreTranslateServer server);

    Task<ApiResponse<string>> TranslateAsync(TranslateRequest request, LibreTranslateServer server);
  }
}