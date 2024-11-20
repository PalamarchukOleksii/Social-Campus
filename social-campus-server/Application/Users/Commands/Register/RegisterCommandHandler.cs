using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.Register
{
    public class RegisterCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher) : ICommandHandler<RegisterCommand>
    {
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string passwordHash = passwordHasher.Hash(request.Password);

            await userRepository.AddAsync(request.Login, passwordHash, request.Email, request.FirstName, request.LastName);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
