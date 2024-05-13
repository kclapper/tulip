using System.ComponentModel.DataAnnotations;

namespace Tulip.Models
{
    public class FloatingChat {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public string OtherUserName { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
