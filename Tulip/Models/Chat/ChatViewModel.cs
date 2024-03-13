using System.ComponentModel.DataAnnotations;

namespace Tulip.Models
{
    public class ChatViewModel
    {
        [Required] 
        public IDictionary<ApplicationUser, MessageHistory> Chats { get; set; }

        public MessageHistory CurrentChat { get; set; }
    }

    public class MessageHistory
    {
        [Required]
        public ApplicationUser OtherUser { get; set; }

        [Required]
        public ICollection<ChatMessage> Messages { get; set; }
    }

}

