using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoriesI18n.Services
{
  public class BackgroundService
  {
    public async Task StartAsync(CancellationToken token)
    {
      await Task.Factory.StartNew(() =>
      {
        while (!token.IsCancellationRequested)
        {
          Console.WriteLine("Running");
          Thread.Sleep(3000);
        }
      });
    }
  }
}