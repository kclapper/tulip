using Gamification.UI.Models;
using System.Threading.Tasks;

namespace Gamification.UI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(ApplicationUser user, string password);
        Task<bool> UserExists(string username);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
        int GetUserId();
        string GetUserEmail();
        Task<ApplicationUser> GetUserByUsername(string username);
    }
}
