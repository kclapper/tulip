
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Tulip.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string recipientId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        // private ApplicationUser getCurrentUser()
        // {
        //     var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //     return _db.ApplicationUsers.Find(userId);
        // }
    }
}