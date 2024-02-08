using Tulip.Data;
using Tulip.Services.Interfaces;
using Tulip.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// PasswordValidationService
/*public class PasswordValidationService : IPasswordValidationService
{
  private UserManager<IdentityUser> userManager;

  public PasswordValidationService(UserManager<IdentityUser> manager)
  {
    userManager = manager;
  }

  public async Task<bool> IsPasswordValid(ClaimsPrincipal user)
  {
    var currentUser = await userManager.GetUserAsync(user);

    if (currentUser == null) return false;
    
    string passwordHash = currentUser.PasswordHash;  
    string pattern = @"^Password@\d{3}$";

    return !Regex.IsMatch(passwordHash, pattern);
  }
}*/