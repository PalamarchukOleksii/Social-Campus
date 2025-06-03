using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Commands.CreatePublication;

public class CreatePublicationCommandHandler(
    IPublicationRepository publicationRepository,
    IUserRepository userRepository) : ICommandHandler<CreatePublicationCommand>
{
    public async Task<Result> Handle(CreatePublicationCommand request, CancellationToken cancellationToken)
    {
        var isUserExist = await userRepository.IsExistByIdAsync(request.CreatorId);
        if (!isUserExist)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with id {request.CreatorId.Value} was not found"));

        await publicationRepository.AddAsync(request.Description, request.CreatorId, request.ImageData);

        return Result.Success();
    }
}