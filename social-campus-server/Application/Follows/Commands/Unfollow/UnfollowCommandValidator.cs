using FluentValidation;

namespace Application.Follows.Commands.Unfollow
{
    public class UnfollowCommandValidator : AbstractValidator<UnfollowCommand>
    {
        public UnfollowCommandValidator()
        {
            RuleFor(f => f.UserLogin)
                .NotEmpty().WithMessage("UserLogn is required");

            RuleFor(f => f.FollowUserLogin)
                .NotEmpty().WithMessage("FollowUserLogin is required");

            RuleFor(f => new { f.UserLogin, f.FollowUserLogin })
                .Must(x => x.UserLogin != x.FollowUserLogin)
                .WithMessage("UserLogin and FollowUserLogin must be different");
        }
    }
}
