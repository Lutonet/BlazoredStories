using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoriesTranslationServices.Models
{
  public class Settings
  {
    public string TranslationFolderPath { get; set; }
    public string SourceLanguageCode { get; set; }
    public List<string> IgnoredLanguageCodes { get; set; }
    public List<LibreTranslateServer> Servers { get; set; }
  }
}