using Tulip.Data;
using Tulip.Models;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tulip.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;
        private readonly ITasksServices _tasksServices;
        private readonly ApplicationDbContext _db;

        public ChatController(ILogger<ChatController> logger, ITasksServices tasksServices, ApplicationDbContext db)
        {
            _tasksServices = tasksServices;
            _db = db;
            _logger = logger;
        }

        private ChatViewModel getAllUserChats() 
        {
            ApplicationUser currentUser = getCurrentUser();

            IEnumerable<ChatMessage> messages = 
                from message in _db.ChatMessages
                where message.Sender.Id.Equals(currentUser.Id) || message.Receiver.Id.Equals(currentUser.Id)
                orderby message.Timestamp descending
                select message;

            IDictionary<ApplicationUser, MessageHistory> chats = new Dictionary<ApplicationUser, MessageHistory>();

            foreach (ChatMessage message in messages)
            {
                ApplicationUser otherUser;
                if (message.Sender.Equals(currentUser))
                {
                    otherUser = message.Receiver;
                }
                else 
                {
                    otherUser = message.Sender;
                }

                if (!chats.ContainsKey(otherUser))
                {
                    var messageHistory = new MessageHistory()
                    {
                        OtherUser = otherUser,
                        Messages = [ message ]
                    };
                    chats.Add(otherUser, messageHistory);
                }
                else 
                {
                    MessageHistory messageHistory;
                    chats.TryGetValue(otherUser, out messageHistory);
                    messageHistory.Messages.Add(message);
                }
            }

            ChatViewModel viewModel = new ChatViewModel()
            {
                Chats = chats
            };

            return viewModel;
        }

        public ActionResult Index()
        {
            return View(getAllUserChats());
        }

        public ActionResult Compose()
        {
            return View(getAllUserChats());
        }

        [Route("Chat/Message/{userId}")]
        public ActionResult Message(string userId)
        {
            ChatViewModel viewModel = getAllUserChats();

            MessageHistory messages;
            viewModel.Chats.TryGetValue(getUserFromId(userId), out messages);

            viewModel.CurrentChat = messages;

            return View(viewModel);
        }

        private ApplicationUser getCurrentUser()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _db.ApplicationUsers.Find(userId);
        }

        private ApplicationUser getUserFromId(string userId)
        {
            return _db.ApplicationUsers.Find(userId);
        }
    }
}