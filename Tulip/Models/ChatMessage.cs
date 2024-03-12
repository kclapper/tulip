using System.ComponentModel.DataAnnotations;

namespace Tulip.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public ApplicationUser Sender { get; set; }

        [Required]
        public ApplicationUser Receiver { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
