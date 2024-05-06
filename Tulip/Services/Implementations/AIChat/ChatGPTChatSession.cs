using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Tulip.Services.Interfaces;

namespace Tulip.Services.Implementations
{
    public class ChatGPTChatSession : IAIChatSession
    {
        private ILogger logger;
        private string endpointUri = "/v1/chat/completions";
        private string model = "gpt-3.5-turbo";
        private HttpClient httpClient;

        public ChatGPTChatSession(ILogger logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<string> SendMessage(string message)
        {
            using StringContent jsonContent = new (
                JsonSerializer.Serialize(new 
                {
                    model = model,
                    messages = new List<object> {
                        new {
                            role = "system",
                            content = "Transcript of a dialog between a user and an SAP expert."
                        },
                        new {
                            role = "user",
                            content = message
                        }
                    }
                    
                }),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await httpClient.PostAsync(endpointUri, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning($"ChatGPT request error" 
                    + $"\nStatus code: {response.StatusCode}"
                    + $"\nMessage: {await response.Content.ReadAsStringAsync()}"
                );
                return "I'm sorry, something went wrong. Can you please try again?";
            }

            var jsonResponse = await getBodyContent(response);
            return jsonResponse["choices"][0]["message"]["content"].ToString();
        }

        private async Task<JObject> getBodyContent(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync();
            return JObject.Parse(stringContent);
        }
    }
}