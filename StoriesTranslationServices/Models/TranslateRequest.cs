using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoriesTranslationServices.Models
{
  public class TranslateRequest
  {
    public string TextToTranslate { get; set; } = string.Empty;
    public string SourceLanguageCode { get; set; } = "en";
    public string TargetLanguageCode { get; set;} = string.Empty;
    public bool IsHtml { get; set; } = false;
    }
}
