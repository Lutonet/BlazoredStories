using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoriesTranslationServices.Models
{
  internal class ServerLanguages
  {
    public LibreTranslateServer Server { get; set; }
    public List<LibreTranslateLanguage> Languages { get; set; }
  }
}