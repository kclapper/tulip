using Tulip.Models;

namespace Tulip.Services.Interfaces
{
    public interface IAIChat
    {
        public bool IsEnabled();
        public void Disable();
        public void Enable();
        public IAIChatSession GetChatSession(ApplicationUser user);
    }
}
