using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsLibrary.Models
{
  public class ApiResponse<T>
  {
    public T? Data { get; set; }
    public bool Successful { get; set; } = true;
    public string ErrorMessage { get; set; } = string.Empty;
  }
}