using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Commands.CreatePublication
{
    public class CreatePublicationCommandHandler(
        IPublicationRepository publicationRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<CreatePublicationCommand>
    {
        public async Task<Result> Handle(CreatePublicationCommand request, CancellationToken cancellationToken)
        {
            bool isUserExist = await userRepository.IsExistByIdAsync(request.CreatorId);
            if (!isUserExist)
            {
                return Result.Failure<TokensDto>(new Error(
                    "User.NotFound",
                    $"User with id {request.CreatorId} was not found"));
            }

            await publicationRepository.AddAsync(request.Description, request.CreatorId, request.ImageData);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
