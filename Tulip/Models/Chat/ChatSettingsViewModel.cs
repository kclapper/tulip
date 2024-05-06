using Tulip.Services.Implementations;

namespace Tulip.Models
{
    public class ChatSettingsViewModel
    {
        public AIChatSystemSelection AIChatSystemSelection { get; set; }

        public bool AIIsEnabled { get; set;}

        public string AIModelFileName { get; set; }

        public string ChatGPTAPIKey { get; set; }
    }
}
