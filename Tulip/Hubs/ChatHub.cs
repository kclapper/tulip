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
                ApplicationUser sender = getUserFromClaimsPrincipal(Context.User);
                ApplicationUser receiver = getUserFromUsername(recipient);

                IEnumerable<ChatMessage> messages = 
                    from chatMessage in db.ChatMessages 
                    where chatMessage.Sender.Id.Equals(sender.Id) || chatMessage.Receiver.Id.Equals(sender.Id)
                    orderby chatMessage.Timestamp descending
                    select chatMessage;

                if (messages.Count() > 0 
                    && messages.First().Timestamp.AddSeconds(10) >= DateTime.Now
                    && messages.First().Sender.Equals(sender))
                {
                    var lastMessage = messages.First();

                    lastMessage.Message += $"\n\n{message}";
                    lastMessage.Timestamp = DateTime.Now;

                    db.Update(lastMessage);
                }
                else 
                {
                    ChatMessage newMessage = new ChatMessage
                    {
                        Timestamp = DateTime.Now,
                        Sender = sender,
                        Receiver = receiver,
                        Message = message
                    };

                    db.Add(newMessage);
                }

                db.SaveChanges();

                string senderId = sender.Id;
                await Clients.User(senderId).SendAsync(ReceiveMessage.Name, Context.User.Identity.Name, message);

                string receiverId = receiver.Id;
                await Clients.User(receiverId).SendAsync(ReceiveMessage.Name, Context.User.Identity.Name, message);

                logger.LogInformation($"[{DateTime.Now.ToLocalTime()}] {sender.UserName}->{receiver.UserName}: {message}");

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