using Humanizer;
using System.ComponentModel;
using static StoriesWeb.Tools.Settings;

namespace StoriesWeb.Models.Dto
{
  public class UserProfileDto
  {
    public string Email { get; set; }
    public string DisplayedName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? CountryId { get; set; }
    public string? PictureUrl { get; set; }
    public Gender Gender { get; set; }
    public string? Info { get; set; }
    public string? Facebook { get; set; }

    public string? Twitter { get; set; }

    public string? Microsoft { get; set; }

    public string? Google { get; set; }
    public bool? MessageNotification { get; set; }
    public bool? PrivateMessageNotification { get; set; }
    public bool? SystemMessageNotification { get; set; }

    [DisplayName("Who can see your email")]
    public AccessRights EmailPrivacy { get; set; } = AccessRights.Private;

    [DisplayName("Who can see your real name")]
    public AccessRights NamePrivacy { get; set; } = AccessRights.Private;

    [DisplayName("Who can see your friends list")]
    public AccessRights FriendsPrivacy { get; set; } = AccessRights.Public;

    [DisplayName("Who can see your Social Media links")]
    public AccessRights SocialMediaLinksPrivacy { get; set; } = AccessRights.Public;

    [DisplayName("Who can see your country?")]
    public AccessRights CountryPrivacy { get; set; } = AccessRights.Public;

    [DisplayName("Who can see your birthday?")]
    public AccessRights BirthDatePrivacy { get; set; } = AccessRights.Public;

    [DisplayName("Who can see your picture?")]
    public AccessRights PictureFilePrivacy { get; set; } = AccessRights.Public;

    [DisplayName("Who can see you online?")]
    public AccessRights LastSeenPrivacy { get; set; } = AccessRights.Public;
  }
}