using Microsoft.AspNetCore.SignalR;
namespace FoodOrderApi.Migrations
{
    public class OrderHub:Hub
    {
        public async Task SendOrderUpdate(string message)
        {
            Console.WriteLine($"Sending message: {message}");
            await Clients.All.SendAsync("ReceiveOrderUpdate", message);
        }
    }
}
