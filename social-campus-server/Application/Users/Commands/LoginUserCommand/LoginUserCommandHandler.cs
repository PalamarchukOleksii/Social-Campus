using Application.Security;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Users.Commands.LoginUserCommand
{
    public class LoginUserCommandHandler(IJwtProvider jwtProvider, IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetUserByEmailAsync(request.Email);

            if (user is null)
            {
                return new LoginUserCommandResponse
                (
                    IsSuccess: false,
                    AccessToken: null,
                    ErrorMessage: "User with that email does not exist"
                );
            }

            bool isPasswordValid = passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return new LoginUserCommandResponse(
                    IsSuccess: false,
                    AccessToken: null,
                    ErrorMessage: "Incorrect password"
                );
            }

            string token = jwtProvider.CreateToken(user);
            return new LoginUserCommandResponse
            (
                IsSuccess: true,
                AccessToken: token,
                ErrorMessage: null
            );
        }
    }
}