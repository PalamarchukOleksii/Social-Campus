using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Commands.CreatePublication
{
    public class CreatePublicationCommandHandler(
        IPublicationRepository publicationRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<CreatePublicationCommand>
    {
        public async Task<Result> Handle(CreatePublicationCommand request, CancellationToken cancellationToken)
        {
            await publicationRepository.AddAsync(request.Description, request.CreatorId, request.Base64ImageData);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
