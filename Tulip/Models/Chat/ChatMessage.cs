using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NuGet.Protocol.Plugins;

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
}
