using Domain.Models.ChatModel;
using Domain.Models.ChatParticipantRoleModel;
using Domain.Models.UserModel;

namespace Domain.Models.ChatParticipantModel
{
    public class ChatParticipant
    {
        public ChatParticipantId Id { get; private set; } = new ChatParticipantId(Guid.Empty);
        public ChatId ChatId { get; private set; } = new ChatId(Guid.Empty);
        public UserId UserId { get; private set; } = new UserId(Guid.Empty);
        public ChatParticipantRoleId RoleId { get; private set; } = new ChatParticipantRoleId(Guid.Empty);
        public virtual ChatParticipantRole? Role { get; }
        public virtual Chat? Chat { get; }
        public virtual User? User { get; }
    }
}