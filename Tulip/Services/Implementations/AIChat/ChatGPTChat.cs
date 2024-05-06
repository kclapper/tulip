using System.Net.Http.Headers;
using Tulip.Models;
using Tulip.Services.Interfaces;

namespace Tulip.Services.Implementations
{
    public class ChatGPTChat : IAIChat
    {
        private bool isEnabled = false;
        private ILogger logger;
        private IConfiguration configuration;
        private HttpClient httpClient;

        public ChatGPTChat(ILogger logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.openai.com"),
            };
        }

        public bool CanBeEnabled()
        {
            return configuration["ChatGPTAPIKey"] != "";
        }

        public void Disable()
        {
            isEnabled = false;
            this.httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public void Enable()
        {
            isEnabled = true;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["ChatGPTAPIKey"]);
        }

        public IAIChatSession GetChatSession(ApplicationUser user)
        {
            return new ChatGPTChatSession(logger, httpClient);
        }

        public bool IsEnabled()
        {
            return isEnabled;
        }
    }
}