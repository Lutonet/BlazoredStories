using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoriesTranslationServices.Models
{
  public class LibreTranslateServer
  {
    public string ServerAddress { get; set; } = string.Empty;
    public int ServerPort { get; set; }
    public string ApiKey { get; set; } = string.Empty;
    public bool UseApiKey { get; set; } = false;
    public List<string>? SupportedLanguages { get; set; } = new List<string>();
  }
}