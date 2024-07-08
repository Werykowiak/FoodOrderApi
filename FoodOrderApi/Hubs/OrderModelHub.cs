using Microsoft.AspNetCore.SignalR;
namespace FoodOrderApi.Hubs
{
    public class OrderModelHub: Hub
    {
        public async Task SendMessage(string user,string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",user, message);
        }
    }
}
