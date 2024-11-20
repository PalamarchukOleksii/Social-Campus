using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.Publications.Commands.CreatePublication
{
    public class CreatePublicationCommandValidator : AbstractValidator<CreatePublicationCommand>
    {
        public CreatePublicationCommandValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.CreatorId)
                .NotNull().WithMessage("CreatorId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("CreatorId must be a valid GUID")
                .MustAsync(async (creatorId, _) => await userRepository.IsExistByIdAsync(creatorId))
                .WithMessage((context, _) => $"User with UserId {context.CreatorId.Value} do not exist");
        }
    }
}
