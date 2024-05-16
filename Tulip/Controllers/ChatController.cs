using Tulip.Data;
using Tulip.Models;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Reflection;
using System.Net;
using Tulip.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Tulip.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> logger;
        private readonly ITasksServices tasksServices;
        private readonly ApplicationDbContext db;
        private readonly IAIChatFactory aiChatFactory;
        private readonly IConfiguration configuration;

        public ChatController(ILogger<ChatController> logger, ITasksServices tasksServices, ApplicationDbContext db, IAIChatFactory aiChatFactory, IConfiguration configuration)
        {
            this.tasksServices = tasksServices;
            this.db = db;
            this.logger = logger;
            this.aiChatFactory = aiChatFactory;
            this.configuration = configuration;
        }

        private ChatViewModel getAllUserChats() 
        {
            ApplicationUser currentUser = getCurrentUser();

            IEnumerable<ChatMessage> messages = 
                from message in db.ChatMessages
                where message.Sender.Id.Equals(currentUser.Id) || message.Receiver.Id.Equals(currentUser.Id)
                orderby message.Timestamp ascending
                select message;

            ChatList chats = new ChatList();

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

                if (!chats.ContainsUser(otherUser))
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
                    MessageHistory messageHistory = chats.GetMessageHistory(otherUser);
                    messageHistory.Messages.Add(message);
                }
            }

            ChatViewModel viewModel = new ChatViewModel()
            {
                Chats = chats,
                AIIsEnabled = aiChatFactory.GetAIChat().IsEnabled()
            };

            return viewModel;
        }

        public ActionResult Index()
        {
            ChatViewModel viewModel = getAllUserChats();

            if (viewModel.Chats.Count() == 0)
            {
                return RedirectToAction("Compose");
            }

            var mostRecentChat = viewModel.Chats.MostRecentChat();

            return RedirectToAction("Message", new { userId = mostRecentChat.OtherUser.Id });
        }

        public ActionResult Compose()
        {
            return View(getAllUserChats());
        }

        [Route("Chat/Message/{userId}")]
        public ActionResult Message(string userId)
        {
            ChatViewModel viewModel = getAllUserChats();

            MessageHistory messages = viewModel.Chats.GetMessageHistory(getUserFromId(userId));

            viewModel.CurrentChat = messages;

            return View(viewModel);
        }

        [Route("Chat/AIMessage")]
        public ActionResult AIMessage()
        {
            IAIChat aiChat = aiChatFactory.GetAIChat();

            if (!aiChat.IsEnabled())
            {
                return RedirectToAction("Index");
            }

            ChatViewModel viewModel = getAllUserChats();
            viewModel.AIIsCurrentChat = true;

            IEnumerable<AIChatMessage> aiMessages = 
                from message in db.AIChatMessages
                where message.User.Id.Equals(getCurrentUser().Id)
                orderby message.Timestamp ascending
                select message;

            ViewData["AIChatMessages"] = aiMessages;

            return View(viewModel);
        }

        private ApplicationUser getCurrentUser()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var users = db.ApplicationUsers.Include(e => e.FloatingChats);
            return users.Where(e => e.Id.Equals(userId)).First();
            // return db.ApplicationUsers.Include(e => e.FloatingChats).Find(userId);
        }

        private ApplicationUser getUserFromId(string userId)
        {
            return db.ApplicationUsers.Find(userId);
        }

        private ApplicationUser getUserFromUsername(string username)
        {
            IEnumerable<ApplicationUser> userQuery = 
                from user in db.ApplicationUsers
                where user.UserName == username
                select user;

            var foundUser = userQuery.Single<ApplicationUser>();

            return foundUser;
        }

        [HttpGet]
        public ActionResult<List<string>> UserSearch([FromQuery] string query) 
        {
            IEnumerable<string> userResults = 
                from user in db.ApplicationUsers
                where user.UserName.ToUpper().Contains(query.ToUpper())
                orderby user.UserName.ToUpper().IndexOf(query.ToUpper()), user.UserName.ToUpper() ascending
                select user.UserName;
            return userResults.Take(5).ToList();
        }

        [HttpGet]
        public ActionResult<List<object>> ChatHistory([FromQuery] string otherUserName) 
        {
            ChatViewModel model = getAllUserChats();
            ApplicationUser otherUser = getUserFromUsername(otherUserName);
            var history = model.Chats.GetMessageHistory(otherUser);

            if (history == null)
            {
                return NotFound();
            }

            IEnumerable<object> messages =
                from message in history.Messages 
                orderby message.Timestamp ascending
                select new {
                    Sender = message.Sender.UserName,
                    Receiver = message.Receiver.UserName,
                    Timestamp = message.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Message = message.Message
                };

            return messages.ToList();
        }

        [HttpGet]
        public ActionResult<List<object>> GetFloatingChats()
        {
            ApplicationUser currentUser = getCurrentUser(); 

            if (currentUser.FloatingChats == null) {
                return new List<object>();
            }

            IEnumerable<object> floatingChats = 
                from chat in currentUser.FloatingChats
                select new {
                    otherUserName = chat.OtherUserName,
                    isActive = chat.IsActive
                }; 

            return floatingChats.ToList();
        }

        [HttpPost]
        public ActionResult OpenFloatingChat([FromBody] FloatingChat floatingChat)
        {
            ApplicationUser currentUser = getCurrentUser(); 
            floatingChat.User = currentUser;

            if (currentUser.FloatingChats == null) {
                db.FloatingChats.Add(floatingChat);
                db.SaveChanges();
                return NoContent();
            }

            foreach (FloatingChat chat in currentUser.FloatingChats)
            {
                if (chat.OtherUserName.Equals(floatingChat.OtherUserName))
                {
                    chat.IsActive = floatingChat.IsActive;
                    db.FloatingChats.Update(chat);
                    db.SaveChanges();
                    return NoContent();
                }
            }

            db.FloatingChats.Add(floatingChat);
            db.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public ActionResult RemoveFloatingChat([FromBody] FloatingChat floatingChat)
        {
            ApplicationUser currentUser = getCurrentUser(); 

            foreach (FloatingChat chat in currentUser.FloatingChats)
            {
                if (chat.OtherUserName.Equals(floatingChat.OtherUserName))
                {
                    currentUser.FloatingChats.Remove(chat);
                }
            }

            db.SaveChanges();
            
            return NoContent();
        }

        [HttpGet]
        public ActionResult Settings()
        {
            ChatSettingsViewModel model = new ChatSettingsViewModel()
            {
                AIChatSystemSelection = getAIChatSystemSelection(),
                AIIsEnabled = aiChatFactory.GetAIChat().IsEnabled()
            };

            switch(model.AIChatSystemSelection)
            {
                case AIChatSystemSelection.LLaMaModelUpload:
                    model.AIModelFilePath = Path.GetFileName(configuration["AIChatModelPath"]); 
                    break;
                case AIChatSystemSelection.ChatGPTAPI:
                    var apiKey = configuration["ChatGPTAPIKey"]; 
                    if (apiKey != "")
                    {
                        model.ChatGPTAPIKey = new String('*', 16);
                    }
                    break;
                default:
                    break;
            }

            return View(model);
        }

        private AIChatSystemSelection getAIChatSystemSelection()
        {
            var selectionName = configuration["AIChatSystem"];

            AIChatSystemSelection selection;
            bool selectionIsValid = Enum.TryParse<AIChatSystemSelection>(selectionName, out selection);

            if (!selectionIsValid)
            {
                logger.LogWarning($"Invalid chat system saved to configuration: {selectionName}");
                selection = AIChatSystemSelection.None;
            }

            return selection;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SelectChatSystem(ChatSettingsViewModel settings)
        {
            AIChatSystemSelection selection = settings.AIChatSystemSelection;

            var selectionName = selection.ToString("G");
            configuration["AIChatSystem"] = selectionName;

            return RedirectToAction("Settings");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveSettings(ChatSettingsViewModel settings)
        {
            AIChatSystemSelection chatSystem = getAIChatSystemSelection();

            switch (chatSystem)
            {
                case AIChatSystemSelection.LLaMaModelUpload:
                    if (settings.AIModelFilePath != null)
                    {
                        var assemblyPath = Assembly.GetExecutingAssembly().Location;
                        var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
                        var modelPath = $"{assemblyDirectory}/{settings.AIModelFilePath}";

                        configuration["AIChatModelPath"] = modelPath;
                    }
                    break;
                case AIChatSystemSelection.ChatGPTAPI:
                    configuration["ChatGPTAPIKey"] = settings.ChatGPTAPIKey;
                    break;
                default:
                    break;
            }

            IAIChat aiChat = aiChatFactory.GetAIChat();
            if (settings.AIIsEnabled && aiChat.CanBeEnabled())
            {
                aiChat.Enable();
            }
            else 
            {
                aiChat.Disable();
            }

            settings.AIIsEnabled = aiChat.IsEnabled();

            return RedirectToAction("Settings");
        }
    }
}