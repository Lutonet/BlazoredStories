using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoriesTranslationServices.Models
{
  public class LibreTranslateLanguage
  {
    public string Code { get; set; }
    public string Name { get; set; }
    public List<string> Targets { get; set; }
  }
}