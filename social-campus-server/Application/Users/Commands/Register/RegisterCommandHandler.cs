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
            bool isEmailUnique = await userRepository.IsEmailUniqueAsync(request.Email);
            if (!isEmailUnique)
            {
                return Result.Failure(new Error(
                "User.NotUniqueEmail",
                $"User with email {request.Email} has already exist"));
            }

            bool isLoginUnique = await userRepository.IsLoginUniqueAsync(request.Login);
            if (!isLoginUnique)
            {
                return Result.Failure(new Error(
                "User.NotUniqueLogin",
                $"User with login {request.Login} has already exist"));
            }

            string passwordHash = passwordHasher.Hash(request.Password);

            await userRepository.AddAsync(request.Login, passwordHash, request.Email, request.FirstName, request.LastName);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
