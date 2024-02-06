using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gamification.UI.Models
{
    public class ApplicationUser : IdentityUser
    {
        // public int Id { get; set; }
        [Required] public string UserId { get; set; }

        [Required]
        [DisplayName("Application Server")]
        public string ApplicationServer { get; set; }

        [Required]
        public int ClientId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Name { get; set; }
        [NotMapped]
        public string RoleId { get; set; }
        [NotMapped]
        public string Role { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> RoleList { get; set; }

    }
}
