using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Users.Commands.Update
{
    public class UpdateCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<UpdateCommand>
    {
        public async Task<Result> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(request.Id);
            if (user is null)
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"User with id {request.Id} was not found"));
            }

            bool isEmailUnique = await userRepository.IsEmailUniqueAsync(request.Email);
            if (!isEmailUnique && request.Email != user.Email)
            {
                return Result.Failure(new Error(
                "User.NotUniqueEmail",
                $"User with email {request.Email} has already exist"));
            }

            bool isLoginUnique = await userRepository.IsLoginUniqueAsync(request.Login);
            if (!isLoginUnique && request.Login != user.Login)
            {
                return Result.Failure(new Error(
                "User.NotUniqueLogin",
                $"User with login {request.Login} has already exist"));
            }

            userRepository.Update(
                user,
                request.Login,
                request.Email,
                request.FirstName,
                request.LastName,
                request.Bio,
                request.ProfileImageData);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
