using Tulip.Services.Interfaces;

namespace Tulip.Services.Implementations
{
    public class ChatGPTChatSession : IAIChatSession
    {
        private ILogger logger;

        public ChatGPTChatSession(ILogger logger)
        {
            this.logger = logger;
        }

        public Task<string> SendMessage(string message)
        {
            return Task.FromResult("This is a test response");
        }
    }
}