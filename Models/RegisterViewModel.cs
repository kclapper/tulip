using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gamification.UI.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string UserId => Username;

        [Required]
        [DisplayName("Application Server")]
        public string ApplicationServer { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> RoleList { get; set; }
        public string RoleSelected { get; set; }

        // New properties for user creation
        [Required]
        [DisplayName("Starting Index")]
        public int StartingIndex { get; set; }

        [Required]
        [DisplayName("Number of Users to Create")]
        public int NumberOfUsersToCreate { get; set; }
    }
}
