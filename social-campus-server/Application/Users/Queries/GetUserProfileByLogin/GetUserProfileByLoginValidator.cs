using FluentValidation;

namespace Application.Users.Queries.GetUserProfileByLogin
{
    public class GetUserProfileByLoginValidator : AbstractValidator<GetUserProfileByLoginQuery>
    {
        public GetUserProfileByLoginValidator()
        {
            RuleFor(u => u.Login)
                .NotEmpty().WithMessage("Login is required");
        }
    }
}
