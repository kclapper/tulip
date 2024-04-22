using Tulip.Models;

namespace Tulip.Services.Interfaces
{
    public interface IAIChat
    {
        public bool IsEnabled();
        public void Disable();
        public void Enable(string modelPath);
        public IAIChatSession GetChatSession(ApplicationUser user);
    }
}
