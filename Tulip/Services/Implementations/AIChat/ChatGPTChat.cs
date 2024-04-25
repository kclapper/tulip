using Tulip.Models;
using Tulip.Services.Interfaces;

namespace Tulip.Services.Implementations
{
    public class ChatGPTChat : IAIChat
    {
        private bool isEnabled = false;
        private ILogger logger;
        private IConfiguration configuration;

        public ChatGPTChat(ILogger logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public bool CanBeEnabled()
        {
            return configuration["ChatGPTAPIKey"] != "";
        }

        public void Disable()
        {
            isEnabled = false;
        }

        public void Enable()
        {
            isEnabled = true;
        }

        public IAIChatSession GetChatSession(ApplicationUser user)
        {
            return new ChatGPTChatSession(logger);
        }

        public bool IsEnabled()
        {
            return isEnabled;
        }
    }
}