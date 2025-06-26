using Domain.Models.ResetPasswordTokenModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories;

public interface IResetPasswordTokenRepository
{
    public Task<ResetPasswordToken?> GetByUserIdAsync(UserId userId);
    public Task<ResetPasswordToken> AddAsync(UserId userPasswordToResetId, string tokenHash);
    public void Remove(ResetPasswordToken token);
}