using System.Net.Sockets;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Tulip.Data;
using Tulip.Models;

using static Tulip.Hubs.ChatEvent;

using LLama.Common;
using LLama;
using LLama.Abstractions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tulip.Hubs
{
    [Authorize]
    public class AIChatHub : ChatHub
    {
        // private LLamaWeights model;
        // private LLamaContext context;
        private ILLamaExecutor executor;
        public AIChatHub(ILLamaExecutor executor, ApplicationDbContext db, ILogger<ChatHub> logger) : base(db, logger)
        {
            this.executor = executor;
        }

        
        override public async Task SendMessage(string recipient, string message)
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

            ChatHistory history = new ChatHistory();
            history.AddMessage(AuthorRole.System, "Transcript of a dialog, between a User and a bot that knows a lot about SAP");

            ChatSession session = new ChatSession(executor, history);
            session.WithOutputTransform(new LLamaTransforms.KeywordTextOutputStreamTransform(
                new string[] { "User:", "Assistant:", "SAP:", "SAP System:" },
                redundancyLength: 8
            ));

            InferenceParams inferenceParams = new InferenceParams() 
            {
                Temperature = 0.9f,
                AntiPrompts = new List<string> { "User:" }
            };

            string response = "";
            await foreach (string text in session.ChatAsync(new ChatHistory.Message(AuthorRole.User, message), inferenceParams))
            {
                response += text;
            }

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

            // try 
            // {
            //     ChatMessage chatMessage = new ChatMessage
            //     {
            //         Timestamp = DateTime.Now,
            //         Sender = getUserFromClaimsPrincipal(Context.User),
            //         Receiver = getUserFromUsername(recipient),
            //         Message = message
            //     };

            //     db.Add(chatMessage);
            //     db.SaveChanges();

            //     string senderId = chatMessage.Sender.Id;
            //     await Clients.User(senderId).SendAsync(ReceiveMessage.Name, Context.User.Identity.Name, message);

            //     string receiverId = chatMessage.Receiver.Id;
            //     await Clients.User(receiverId).SendAsync(ReceiveMessage.Name, Context.User.Identity.Name, message);

            //     logger.LogInformation($"[{chatMessage.Timestamp.ToLocalTime()}] {chatMessage.Sender.UserName}->{chatMessage.Receiver.UserName}: {chatMessage.Message}");

            //     await Clients.Caller.SendAsync(SentMessage.Name, receiverId);
            // } 
            // catch (Exception e) 
            // {
            //     await Clients.Caller.SendAsync(SendError.Name);
            //     logger.LogWarning(
            //         $"Error sending message to"
            //         + $"Sender: {Context.User.Identity.Name}"
            //         + $"Recipient: {recipient}"
            //         + $"Message: {message}"
            //         + $"Error Type: {e.GetType}"
            //         + $"Error msg: {e.Message}"
            //     );
            // }
        }
    }
}