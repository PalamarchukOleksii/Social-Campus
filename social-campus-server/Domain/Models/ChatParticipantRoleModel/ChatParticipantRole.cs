using Domain.Models.ChatParticipantModel;

namespace Domain.Models.ChatParticipantRoleModel
{
    public class ChatParticipantRole
    {
        public ChatParticipantRoleId Id { get; private set; } = new ChatParticipantRoleId(Guid.Empty);
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public virtual ICollection<ChatParticipant>? ChatParticipants { get; }
    }
}