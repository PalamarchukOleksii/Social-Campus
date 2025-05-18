using Domain.Models.UserModel;

namespace Domain.Models.ReportModel
{
    public class Report
    {
        public ReportId Id { get; set; } = new ReportId(Guid.NewGuid());
        public UserId ReporterId { get; set; } = new UserId(Guid.Empty);
        public virtual User? Reporter { get; }
        public string TargetType { get; private set; } = string.Empty;
        public Guid TargetId { get; private set; } = Guid.Empty;
        public string Reason { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public bool Resolved { get; private set; } = false;
        public UserId ResolvedById { get; set; } = new UserId(Guid.Empty);
        public virtual User? Resolver { get; }
        public string? ResolutionComment { get; }
        public DateTime? ResolvedAt { get; }
    }
}
