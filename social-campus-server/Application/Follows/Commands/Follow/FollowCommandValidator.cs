using FluentValidation;

namespace Application.Follows.Commands.Follow
{
    public class FollowCommandValidator : AbstractValidator<FollowCommand>
    {
        public FollowCommandValidator()
        {
            RuleFor(f => f.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");

            RuleFor(f => f.FollowUserId)
                .NotEmpty().WithMessage("FollowUserId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("FollowUserId must be a valid GUID");

            RuleFor(f => new { f.UserId, f.FollowUserId })
                .Must(x => x.UserId != x.FollowUserId)
                .WithMessage("UserId and FollowUserId must be different");
        }
    }
}
