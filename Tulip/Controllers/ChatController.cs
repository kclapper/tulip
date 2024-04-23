using Tulip.Data;
using Tulip.Models;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Reflection;
using System.Net;
using System.Collections.Immutable;
using System.Collections.Specialized;

namespace Tulip.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> logger;
        private readonly ITasksServices tasksServices;
        private readonly ApplicationDbContext db;
        private readonly IAIChat aiChat;
        private readonly IConfiguration configuration;

        public ChatController(ILogger<ChatController> logger, ITasksServices tasksServices, ApplicationDbContext db, IAIChat aiChat, IConfiguration configuration)
        {
            this.tasksServices = tasksServices;
            this.db = db;
            this.logger = logger;
            this.aiChat = aiChat;
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
                AIIsEnabled = aiChat.IsEnabled()
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
            if (!aiChat.IsEnabled())
            {
                return RedirectToAction("Index");
            }

            ChatViewModel viewModel = getAllUserChats();

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
            return db.ApplicationUsers.Find(userId);
        }

        private ApplicationUser getUserFromId(string userId)
        {
            return db.ApplicationUsers.Find(userId);
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
        public ActionResult Settings()
        {
            var modelName = Path.GetFileName(configuration["AIChatModelPath"]);

            ChatSettingsViewModel model = new ChatSettingsViewModel()
            {
                AIIsEnabled = aiChat.IsEnabled(),
                AIModelFileName = modelName
            };

            logger.LogDebug($"{model.AIModelFileName}");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveSettings(IFormFile modelUpload, ChatSettingsViewModel settings)
        {
            if (settings.AIIsEnabled)
            {
                if (modelUpload == null && configuration["AIChatModelPath"] == "")
                {
                    logger.LogWarning($"Request to Enable AI Chat without providing a model");
                    return RedirectToAction("Settings");
                }

                if (modelUpload != null)
                {
                    var modelFileName = Path.GetFileName(modelUpload.FileName);
                    modelFileName = WebUtility.HtmlEncode(modelFileName);

                    var assemblyPath = Assembly.GetExecutingAssembly().Location;
                    var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
                    var modelPath = $"{assemblyDirectory}/{modelFileName}";

                    using (var stream = System.IO.File.Create(modelPath))
                    {
                        await modelUpload.CopyToAsync(stream);
                    }

                    configuration["AIChatModelPath"] = modelPath;
                }

                aiChat.Enable();
                logger.LogInformation($"Enabled AI chat system with model at: {configuration["AIChatModelPath"]}");
            }
            else 
            {
                aiChat.Disable();
                logger.LogInformation("Disabled AI chat system");
            }

            logger.LogInformation($"{modelUpload}\n{settings}");
            return RedirectToAction("Settings");
        }
    }
}