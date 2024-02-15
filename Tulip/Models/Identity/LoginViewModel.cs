using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tulip.Models
{
    public class LoginViewModel
    {
        [Required] public string UserId { get; set; }

        //[Required]
        //[DisplayName("Application Server")]
        //public string ApplicationServer { get; set; }

        //[Required] 
        //public int ClientId { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
    }
}
