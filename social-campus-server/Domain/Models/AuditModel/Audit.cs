using Domain.Enums;
using Domain.Models.UserModel;

namespace Domain.Models.AuditModel
{
    public class Audit
    {
        public AuditId Id { get; private set; } = new AuditId(Guid.Empty);
        public string TableName { get; private set; } = string.Empty;
        public string FieldName { get; private set; } = string.Empty;
        public Guid RecordId { get; private set; } = Guid.Empty;
        public string OldValue { get; private set; } = string.Empty;
        public string NewValue { get; private set; } = string.Empty;
        public AuditAction Action { get; }
        public UserId PerformerId { get; private set; } = new UserId(Guid.Empty);
        public string PerformerIp { get; private set; } = string.Empty;
        public DateTime PerformedAt { get; private set; } = DateTime.UtcNow;
    }
}