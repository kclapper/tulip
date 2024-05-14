using System.ComponentModel.DataAnnotations;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Tulip.Models
{
    public class ChatViewModel
    {
        [Required] 
        public ChatList Chats { get; set; }

        public MessageHistory CurrentChat { get; set; }
        public bool AIIsEnabled { get; set;}
        public bool AIIsCurrentChat { get; set;} = false;
    }

    public class ChatList : IEnumerable<KeyValuePair<ApplicationUser, MessageHistory>> {
        private IDictionary<ApplicationUser, MessageHistory> chatsByUser = new Dictionary<ApplicationUser, MessageHistory>();

        public bool ContainsUser(ApplicationUser user)
        {
            return chatsByUser.ContainsKey(user);
        }

        public MessageHistory GetMessageHistory(ApplicationUser user)
        {
            MessageHistory history;
            chatsByUser.TryGetValue(user, out history);
            return history;
        }

        public void Add(ApplicationUser user, MessageHistory history)
        {
            chatsByUser.Add(user, history);
        }

        public MessageHistory MostRecentChat()
        {
            var chatList = GetEnumerator();
            chatList.MoveNext();
            return chatList.Current.Value;
        }

        public int Count()
        {
            return chatsByUser.Count;
        }

        public IEnumerator<KeyValuePair<ApplicationUser, MessageHistory>> GetEnumerator()
        {
            List<KeyValuePair<ApplicationUser, MessageHistory>> chatList = chatsByUser.ToList();
            chatList.Sort((first, second) => second.Value.Messages.Last().Timestamp.CompareTo(first.Value.Messages.Last().Timestamp));
            return chatList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            IEnumerator<KeyValuePair<ApplicationUser, MessageHistory>> enumerator = GetEnumerator();
            return enumerator;
        }
    }

    public class MessageHistory
    {
        [Required]
        public ApplicationUser OtherUser { get; set; }

        [Required]
        public ICollection<ChatMessage> Messages { get; set; }
    }

}

