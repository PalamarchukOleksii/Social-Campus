using FluentValidation;

namespace Application.Users.Queries.GetUserByLogin
{
    public class GetUserByLoginValidator : AbstractValidator<GetUserByLoginQuery>
    {
        public GetUserByLoginValidator()
        {
            RuleFor(u => u.Login)
                .NotEmpty().WithMessage("Login is required");
        }
    }
}
