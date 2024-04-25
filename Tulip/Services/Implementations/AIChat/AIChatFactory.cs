using System.ComponentModel.DataAnnotations;
using Tulip.Models;
using Tulip.Services.Interfaces;

namespace Tulip.Services.Implementations
{
    public class AIChatFactory : IAIChatFactory
    {
        private IAIChat aiChat = new DummyAIChat();
        private AIChatSystemSelection selection = AIChatSystemSelection.None;
        private IConfiguration configuration;
        private ILogger<IAIChat> logger;

        public AIChatFactory(IConfiguration configuration, ILogger<IAIChat> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public IAIChat GetAIChat()
        {
            AIChatSystemSelection aiSelection;  
            bool selectionIsValid = Enum.TryParse<AIChatSystemSelection>(configuration["AIChatSystem"], out aiSelection);

            if (!selectionIsValid)
            {
                string configuredAISystem = configuration["AIChatSystem"];
                logger.LogWarning($"Attempted to select invalid AI Chat system: {configuredAISystem}");
                return aiChat;
            }

            if (selection.Equals(aiSelection))
            {
                return aiChat;
            }

            selection = aiSelection;

            bool wasEnabled = aiChat.IsEnabled();
            aiChat.Disable();

            switch (aiSelection)
            {
                case AIChatSystemSelection.None:
                    aiChat = new DummyAIChat();
                    break;

                case AIChatSystemSelection.LLaMaModelUpload:
                    aiChat = new LLamaChat(logger, configuration);
                    break;

                case AIChatSystemSelection.ChatGPTAPI:
                    aiChat = new ChatGPTChat(logger, configuration);
                    break;

                default:
                    string configuredAISystem = configuration["AIChatSystem"];
                    logger.LogWarning($"Attempted to select invalid AI Chat system: {configuredAISystem}");
                    throw new Exception($"Attempted to select invalid AI Chat system: {configuredAISystem}");
            }

            if (wasEnabled && aiChat.CanBeEnabled())
            {
                aiChat.Enable();
            }
            else 
            {
                aiChat.Disable();
            }

            return aiChat;
        }
    }

    public enum AIChatSystemSelection 
    {
        [Display(Name = "None (Disabled)")]
        None,

        [Display(Name = "LLaMa Model Upload")]
        LLaMaModelUpload, 

        [Display(Name = "ChatGPT Web API")]
        ChatGPTAPI,
    }

    class DummyAIChat : IAIChat
    {
        public void Disable()
        {
            return;
        }

        public void Enable()
        {
            return;
        }

        public IAIChatSession GetChatSession(ApplicationUser user)
        {
            throw new NotImplementedException("No AI Chat system has been selected");
        }

        public bool CanBeEnabled()
        {
            return true;
        }

        public bool IsEnabled()
        {
            return false;
        }
    }
}