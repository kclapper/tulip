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
        public string SenderId { get; set; }
        [Required]
        public ApplicationUser Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public ApplicationUser Receiver { get; set; }

        [Required]
        public string Message { get; set; }
    }

    public class AIChatMessage
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public bool IsFromUser { get; set; }
    }
}
