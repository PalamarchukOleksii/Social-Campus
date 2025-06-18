using FluentValidation;

namespace Application.Users.Queries.SearchUsers;

public class SearchUsersQueryHandlerValidator : AbstractValidator<SearchUsersQuery>
{
    public SearchUsersQueryHandlerValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotEmpty().WithMessage("SearchTerm is required");

        RuleFor(f => f.Page)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0.");

        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Count must be 100 or less.");
    }
}