using Tulip.Data;
using Tulip.Models;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Reflection;
using System.Net;
using Tulip.Services.Implementations;
using Microsoft.Identity.Client;

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
            ChatSettingsViewModel model = new ChatSettingsViewModel()
            {
                AIChatSystemSelection = getAIChatSystemSelection(),
                AIIsEnabled = aiChatFactory.GetAIChat().IsEnabled()
            };

            switch(model.AIChatSystemSelection)
            {
                case AIChatSystemSelection.LLaMaModelUpload:
                    model.AIModelFileName = Path.GetFileName(configuration["AIChatModelPath"]); 
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
        public async Task<ActionResult> SaveSettings(IFormFile modelUpload, ChatSettingsViewModel settings)
        {
            AIChatSystemSelection chatSystem = getAIChatSystemSelection();

            switch (chatSystem)
            {
                case AIChatSystemSelection.LLaMaModelUpload:
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
                    break;
                case AIChatSystemSelection.ChatGPTAPI:
                    logger.LogInformation("Saving chat gpt api key settings");
                    logger.LogInformation($"{settings.ChatGPTAPIKey}");
                    configuration["ChatGPTAPIKey"] = settings.ChatGPTAPIKey;
                    break;
                default:
                    break;
            }

            if (settings.AIIsEnabled)
            {
                aiChatFactory.GetAIChat().Enable();
            }
            else 
            {
                aiChatFactory.GetAIChat().Disable();
            }

            return RedirectToAction("Settings");
        }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<ActionResult> SaveSettings(ChatSettingsViewModel settings)
        // {
        //     configuration["AIChatSystem"] = settings.AIChatSystemSelection.ToString("G");

        //     switch (settings.AIChatSystemSelection)
        //     {
        //         case AIChatSystemSelection.ChatGPTAPI:
        //             logger.LogInformation("Saving chat gpt api key settings");
        //             logger.LogInformation($"{settings.ChatGPTAPIKey}");
        //             configuration["ChatGPTAPIKey"] = settings.ChatGPTAPIKey;
        //             break;
        //         default:
        //             break;
        //     }

        //     return RedirectToAction("Settings");
        // }
    }
}