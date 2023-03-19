using StoriesWeb.Models.Dto;

namespace StoriesWeb.Services
{
  public interface IHelperService
  {
    HelpDto GetHelp(string name);
  }
}