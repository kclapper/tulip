using System.Net.Sockets;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Tulip.Data;
using Tulip.Models;

using static Tulip.Hubs.ChatEvent;

namespace Tulip.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private ApplicationDbContext db;
        private ILogger<ChatHub> logger;
        public ChatHub(ApplicationDbContext db, ILogger<ChatHub> logger) : base()
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task SendMessage(string recipient, string message)
        {
            try 
            {
                ChatMessage chatMessage = new ChatMessage
                {
                    Timestamp = DateTime.Now,
                    Sender = getUserFromClaimsPrincipal(Context.User),
                    Receiver = getUserFromUsername(recipient),
                    Message = message
                };

                db.ChatMessages.Add(chatMessage);
                db.SaveChanges();

                await Clients.User(recipient).SendAsync(ReceiveMessage.Name, Context.User.Identity.Name, message);

                logger.LogInformation($"[{chatMessage.Timestamp.ToLocalTime()}] {chatMessage.Sender.Name}->{chatMessage.Receiver.Name}: {chatMessage.Message}");
            } 
            catch (Exception e) 
            {
                await Clients.Caller.SendAsync(SendError.Name);
                logger.LogWarning(
                    $"Error sending message to"
                    + $"Sender: ${Context.User.Identity.Name}"
                    + $"Recipient: ${recipient}"
                    + $"Message: ${message}"
                    + $"Error Type: ${e.GetType}"
                    + $"Error msg: ${e.Message}"
                );
            }
        }

        private ApplicationUser getUserFromClaimsPrincipal(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            return db.ApplicationUsers.Find(userId);
        }

        private ApplicationUser getUserFromUsername(string username)
        {
            IEnumerable<ApplicationUser> userQuery = 
                from user in db.ApplicationUsers
                where user.UserId == username
                select user;

            return userQuery.Single<ApplicationUser>();
        }
    }

    class ChatEvent
    {
        public static ChatEvent ReceiveMessage = new ChatEvent("ReceiveMessage"); 
        public static ChatEvent SendError = new ChatEvent("SendError");

        public string Name { get; }

        private ChatEvent(string name)
        {
            Name = name;
        }
    }
}