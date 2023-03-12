using Microsoft.AspNetCore.SignalR;

namespace StoriesWeb.Hubs
{
  public class IndexHub : Hub
  {
    public async Task SendMessage(string user, string message)
    {
      await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
  }
}