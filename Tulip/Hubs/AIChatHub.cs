using System.Net.Sockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Tulip.Data;
using Tulip.Models;
using static Tulip.Hubs.ChatEvent;
using Tulip.Services.Interfaces;

namespace Tulip.Hubs
{
    [Authorize]
    public class AIChatHub : ChatHub
    {
        private IAIChatFactory chatFactory;
        public AIChatHub(IAIChatFactory chatFactory, ApplicationDbContext db, ILogger<ChatHub> logger) : base(db, logger)
        {
            this.chatFactory = chatFactory;
        }

        override public async Task<bool> AISystemStatus()
        {
            return chatFactory.GetAIChat().IsEnabled();
        }
        
        override public async Task SendMessage(string recipient, string message)
        {
            try 
            {
                ApplicationUser currentUser = getUserFromClaimsPrincipal(Context.User);

                IEnumerable<AIChatMessage> userAIMessages = 
                    from aiMessage in db.AIChatMessages 
                    where aiMessage.User.Id.Equals(currentUser.Id)
                    orderby aiMessage.Timestamp ascending
                    select aiMessage;
        
                AIChatMessage newMessage = new AIChatMessage()
                {
                    User = currentUser,
                    Timestamp = DateTime.Now,
                    Message = message,
                    IsFromUser = true,
                };

                db.Add(newMessage);
                db.SaveChanges();

                await Clients.Caller.SendAsync(ReceiveMessage.Name, Context.User.Identity.Name, message);

                IAIChat chat = chatFactory.GetAIChat();
                string response = await chat.GetChatSession(currentUser).SendMessage(message);

                AIChatMessage aiReponse = new AIChatMessage()
                {
                    User = currentUser,
                    Timestamp = DateTime.Now,
                    Message = response,
                    IsFromUser = false,
                };

                db.Add(aiReponse);
                db.SaveChanges();

                logger.LogInformation($"{currentUser.UserName}: {message}\nAI Reponse: {response}");

                await Clients.Caller.SendAsync(ReceiveMessage.Name, "Tulip Bot", response);
            }
            catch (Exception e) 
            {
                await Clients.Caller.SendAsync(SendError.Name);
                logger.LogWarning(
                    $"Error sending message to Tulip Bot"
                    + $"Sender: {Context.User.Identity.Name}"
                    + $"Message: {message}"
                    + $"Error Type: {e.GetType}"
                    + $"Error msg: {e.Message}"
                );
            }
        }
    }
}