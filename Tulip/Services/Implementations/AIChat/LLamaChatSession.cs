using LLama;
using LLama.Common;
using Tulip.Services.Interfaces;

namespace Tulip.Services.Implementations
{
    public class LLamaChatSession: IAIChatSession
    {
        private readonly ILogger logger;
        private readonly ChatSession session;
        private readonly InferenceParams inferenceParams;

        public LLamaChatSession(ILogger logger, ChatSession session, InferenceParams inferenceParams)
        {
            this.logger = logger;
            this.session = session;
            this.inferenceParams = inferenceParams;
        }

        public async Task<string> SendMessage(string message)
        {
            string response = "";
            await foreach (string text in session.ChatAsync(new ChatHistory.Message(AuthorRole.User, message), inferenceParams))
            {
                response += text;
            }
            return response;
        }
    }
}