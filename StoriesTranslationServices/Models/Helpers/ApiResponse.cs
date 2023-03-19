﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoriesTranslationServices.Models.Helpers
{
  public class ApiResponse<T>
  {
    public bool Success { get; set; } = true;
    public string Error { get; set; }
    public T? Data { get; set; }
  }
}