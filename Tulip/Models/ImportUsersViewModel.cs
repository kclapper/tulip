using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tulip.Models
{
    public class ImportUsersViewModel
    {
        [Required]
        public string ApplicationServer { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public IFormFile File { get; set; }

    }
}
