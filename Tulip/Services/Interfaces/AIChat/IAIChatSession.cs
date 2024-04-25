namespace Tulip.Services.Interfaces
{
    public interface IAIChatSession
    {
        public Task<string> SendMessage(string message);
    }
}
