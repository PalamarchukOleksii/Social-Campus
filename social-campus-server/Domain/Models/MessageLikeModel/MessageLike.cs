using Domain.Models.MessageModel;
using Domain.Models.UserModel;

namespace Domain.Models.MessageLikeModel
{
    public class MessageLike
    {
        public MessageLikeId Id { get; private set; } = new MessageLikeId(Guid.Empty);
        public MessageId MessageId { get; private set; } = new MessageId(Guid.Empty);
        public UserId UserId { get; private set; } = new UserId(Guid.Empty);
        public virtual Message? Message { get; }
        public virtual User? User { get; }
    }
}