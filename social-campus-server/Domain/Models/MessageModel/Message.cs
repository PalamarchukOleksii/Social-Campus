using Domain.Models.ChatModel;
using Domain.Models.MessageLikeModel;
using Domain.Models.UserModel;

namespace Domain.Models.MessageModel
{
    public class Message
    {
        public MessageId Id { get; private set; } = new MessageId(Guid.Empty);
        public string Text { get; set; } = string.Empty;
        public string AttachedImageData { get; private set; } = string.Empty;
        public DateTime SendDateTime { get; private set; } = DateTime.UtcNow;
        public ChatId ChatId { get; set; } = new ChatId(Guid.Empty);
        public virtual Chat? Chat { get; }
        public UserId SenderId { get; set; } = new UserId(Guid.Empty);
        public virtual User? Sender { get; }

        public MessageId ReplyToMessageId { get; private set; } = new MessageId(Guid.Empty);
        public virtual Message? ReplyToMessage { get; }
        public virtual ICollection<Message>? RepliedMessages { get; }
        public virtual ICollection<MessageLike>? MessageLikes { get; }
    }
}
