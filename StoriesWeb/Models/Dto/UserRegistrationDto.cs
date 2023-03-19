using Microsoft.AspNetCore.Components.Forms;
using Org.BouncyCastle.Bcpg.OpenPgp;
using static StoriesWeb.Tools.Settings;

namespace StoriesWeb.Models.Dto
{
  public class UserRegistrationDto
  {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayedName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public int CountryId { get; set; }
    public IBrowserFile Picture { get; set; }
    public Gender Gender { get; set; }
  }
}