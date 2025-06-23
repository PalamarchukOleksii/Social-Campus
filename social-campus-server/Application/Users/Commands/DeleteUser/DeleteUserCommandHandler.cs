using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with id {request.UserId.Value} was not found"));

        if (!await userRepository.IsExistByIdAsync(request.CallerId))
            return Result.Failure(new Error(
                "User.NotFound",
                $"Caller with id {request.CallerId.Value} was not found"));

        if (request.UserId != request.CallerId)
            return Result.Failure(new Error(
                "User.NoDeletePermission",
                $"User with UserId {request.CallerId.Value} do not have permission to delete profile of user with UserId {request.UserId.Value}"));

        await userRepository.DeleteAsync(user);

        return Result.Success();
    }
}