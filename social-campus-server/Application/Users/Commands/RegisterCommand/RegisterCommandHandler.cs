using Application.Abstractions.Data;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Entities;
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

            User user = new User
            {
                Login = request.Login,
                PasswordHash = passwordHash,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            userRepository.AddAsync(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
