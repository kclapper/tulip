using System.ComponentModel.DataAnnotations;

namespace Tulip.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
