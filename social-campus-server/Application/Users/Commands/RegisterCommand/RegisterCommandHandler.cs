using Application.Abstractions.Data;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using MediatR;

namespace Application.Users.Commands.RegisterCommand
{
    public class RegisterCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher) : IRequestHandler<RegisterCommandRequest>
    {
        public async Task Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
            string passwordHash = passwordHasher.Hash(request.Password);

            await userRepository.AddAsync(request.Login, passwordHash, request.Email, request.FirstName, request.LastName);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
