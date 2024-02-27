using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tulip.Models
{
    public class ImportUsersViewModel
    {
        [Required]
        public IFormFile File { get; set; }

    }
}
