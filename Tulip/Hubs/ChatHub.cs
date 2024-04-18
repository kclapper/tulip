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
        protected ApplicationDbContext db;
        protected ILogger<ChatHub> logger;
        public ChatHub(ApplicationDbContext db, ILogger<ChatHub> logger) : base()
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task<string> GetCurrentUser()
        {
            ApplicationUser currentUser = getUserFromClaimsPrincipal(Context.User);
            return currentUser.UserName;
        }

        public virtual async Task SendMessage(string recipient, string message)
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

                db.Add(chatMessage);
                db.SaveChanges();

                string senderId = chatMessage.Sender.Id;
                await Clients.User(senderId).SendAsync(ReceiveMessage.Name, Context.User.Identity.Name, message);

                string receiverId = chatMessage.Receiver.Id;
                await Clients.User(receiverId).SendAsync(ReceiveMessage.Name, Context.User.Identity.Name, message);

                logger.LogInformation($"[{chatMessage.Timestamp.ToLocalTime()}] {chatMessage.Sender.UserName}->{chatMessage.Receiver.UserName}: {chatMessage.Message}");

                await Clients.Caller.SendAsync(SentMessage.Name, receiverId);
            } 
            catch (Exception e) 
            {
                await Clients.Caller.SendAsync(SendError.Name);
                logger.LogWarning(
                    $"Error sending message to"
                    + $"Sender: {Context.User.Identity.Name}"
                    + $"Recipient: {recipient}"
                    + $"Message: {message}"
                    + $"Error Type: {e.GetType}"
                    + $"Error msg: {e.Message}"
                );
            }
        }

        protected ApplicationUser getUserFromClaimsPrincipal(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = db.ApplicationUsers.Find(userId);
            return user;
        }

        protected ApplicationUser getUserFromUsername(string username)
        {
            IEnumerable<ApplicationUser> userQuery = 
                from user in db.ApplicationUsers
                where user.UserName == username
                select user;

            var foundUser = userQuery.Single<ApplicationUser>();

            return foundUser;
        }
    }

    class ChatEvent
    {
        public static ChatEvent ReceiveMessage = new ChatEvent("ReceiveMessage"); 
        public static ChatEvent SendError = new ChatEvent("SendError");
        public static ChatEvent CurrentUser = new ChatEvent("GetCurrentUser");
        public static ChatEvent SentMessage = new ChatEvent("MessageSent");

        public string Name { get; }

        private ChatEvent(string name)
        {
            Name = name;
        }
    }
}