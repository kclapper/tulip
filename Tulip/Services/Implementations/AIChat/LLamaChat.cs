using LLama;
using LLama.Abstractions;
using LLama.Common;
using Microsoft.Extensions.Logging.Abstractions;
using Tulip.Models;
using Tulip.Services.Interfaces;

namespace Tulip.Services.Implementations
{
    public class LLamaChat: IAIChat
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private InteractiveExecutor modelExecutor;

        public LLamaChat(ILogger logger, IConfiguration configuration)
        {
            this.configuration = configuration; 
            this.logger = logger;
        }

        public bool CanBeEnabled()
        {
            return configuration["AIChatModelPath"] != "";
        }

        public bool IsEnabled()
        {
            return modelExecutor != null;
        }

        public void Enable()
        {
            if (IsEnabled())
            {
                Disable();
            }

            var modelPath = configuration["AIChatModelPath"];

            var modelParams = new ModelParams(modelPath);
            var model = LLamaWeights.LoadFromFile(modelParams);
            var context = model.CreateContext(modelParams, NullLogger.Instance);

            this.modelExecutor = new InteractiveExecutor(context, NullLogger.Instance);
        }

        public void Disable()
        {
            this.modelExecutor = null;
            GC.Collect();
        }

        public IAIChatSession GetChatSession(ApplicationUser user)
        {
            LLamaChatSessionBuilder builder = new LLamaChatSessionBuilder(this);

            ChatHistory history = new ChatHistory();
            history.AddMessage(AuthorRole.System, "Transcript of a dialog between a user and an SAP expert.");
            // Here is where we'd add the user's chat history.
            builder.setHistory(history);

            builder.setOutputTransform(new LLamaTransforms.KeywordTextOutputStreamTransform(
                new string[] { "User:", "Assistant:", "SAP:", "SAP System:", "Bot:", "Expert:" },
                redundancyLength: 10
            ));

            builder.setInferenceParams(new InferenceParams() 
                {
                    Temperature = 0.9f,
                    AntiPrompts = new List<string> { "User:" }
                }
            );

            return builder.build();    
        }

        // This should probably be an inner class of LLamaChatSession instead
        // Or the LLamaChatSessionBuilder should be an available service?
        private class LLamaChatSessionBuilder {
            private LLamaChat chat;
            private ILogger logger;
            private InferenceParams inferenceParams;
            private ChatHistory history; 
            private ITextStreamTransform transform;


            public LLamaChatSessionBuilder(LLamaChat chat) 
            {
                this.logger = chat.logger;
                this.chat = chat;
            }

            public LLamaChatSessionBuilder setInferenceParams(InferenceParams inferenceParams)
            {
                this.inferenceParams = inferenceParams;
                return this;
            }

            public LLamaChatSessionBuilder setHistory(ChatHistory history)
            {
                this.history = history; 
                return this;
            }

            public LLamaChatSessionBuilder setOutputTransform(ITextStreamTransform transform)
            {
                this.transform = transform; 
                return this;
            }

            public LLamaChatSession build()
            {
                if (history == null)
                {
                    history = new ChatHistory();
                }

                if (inferenceParams == null)
                {
                    inferenceParams = new InferenceParams();
                }

                var session = new ChatSession(chat.modelExecutor, history);

                if (transform != null)
                {
                    session.WithOutputTransform(transform);
                }

                return new LLamaChatSession(logger, session, inferenceParams);
            }

        }

    }

}