using Application.Data;
using Application.Security;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Users.Commands.RegisterUserCommand
{
    public class RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommandRequest, RegisterUserCommandResponse>
    {
        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
        {
            User? existingUser = await userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new RegisterUserCommandResponse(
                    IsSuccess: false,
                    ErrorMessage: "A user with this email already exists",
                    User: default
                );
            }

            existingUser = await userRepository.GetUserByLoginAsync(request.Login);
            if (existingUser != null)
            {
                return new RegisterUserCommandResponse(
                    IsSuccess: false,
                    ErrorMessage: "A user with this login already exists",
                    User: default
                );
            }

            string passwordHash = passwordHasher.Hash(request.Password);

            User? user = await userRepository.CreateUserAsync(request.Login, passwordHash, request.Email, request.FirstName, request.LastName);

            if (user is null)
            {
                return new RegisterUserCommandResponse(
                IsSuccess: false,
                ErrorMessage: "A user with this email already exists",
                User: default
                );
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new RegisterUserCommandResponse(
                IsSuccess: true,
                ErrorMessage: null,
                User: user
            );
        }
    }
}
