using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoriesTranslationServices.Models
{
  internal class ServerForLanguage
  {
    public string LanguageCode { get; set; }
    public List<LibreTranslateServer> Servers { get; set; } = new List<LibreTranslateServer>();
  }
}