using StoriesWeb.Models.Dto;
using System.Security.Cryptography.Xml;

namespace StoriesWeb.Services
{
  public class HelperService : IHelperService
  {
    private List<HelpDto> helps = new List<HelpDto>()
        {
          new HelpDto(){Name = "RegistrationEmail", Title="Registration Email", Body="Enter valid email address"},
          new HelpDto(){Name = "RegistrationPassword", Title="Password", Body="PasswordHelp"},
          new HelpDto(){Name = "RegistrationPasswordCheck", Title="Password confirmation", Body="PasswordConfirmationHelp"},
          new HelpDto(){Name = "RegistrationFirstname", Title="First name", Body="FirstnameHelp"},
          new HelpDto(){Name = "RegistrationLastname", Title="Last name", Body="LastnameHelp"},
          new HelpDto(){Name = "RegistrationDisplayedname", Title="Displayed name", Body="DisplayednameHelp"},
          new HelpDto(){Name = "RegistrationGender", Title="Gender", Body="GenderHelp"},
          new HelpDto(){Name = "RegistrationCountry", Title="Country", Body="CountryHelp"},
          new HelpDto(){Name = "RegistrationPicture", Title="Profile picture", Body="PictureHelp"}
        };

    public HelpDto GetHelp(string name)
    {
      if (helps.FirstOrDefault(s => s.Name == name) == null)
      {
        return new();
      }
      return helps.FirstOrDefault(s => s.Name == name);
    }
  }
}