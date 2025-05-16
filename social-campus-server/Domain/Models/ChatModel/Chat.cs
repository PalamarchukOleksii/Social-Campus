using Domain.Models.ChatParticipantModel;
using Domain.Models.MessageModel;

namespace Domain.Models.ChatModel
{
    public class Chat
    {
        public ChatId Id { get; private set; } = new ChatId(Guid.Empty);
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string ImageData { get; private set; } = string.Empty;
        public bool IsGroupChat { get; set; }
        public virtual ICollection<ChatParticipant>? Participants { get; }
        public virtual ICollection<Message>? Messages { get; }
    }
}