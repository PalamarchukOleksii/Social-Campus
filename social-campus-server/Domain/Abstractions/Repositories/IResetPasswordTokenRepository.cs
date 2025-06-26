using Domain.Models.ResetPasswordTokenModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories;

public interface IResetPasswordTokenRepository
{
    public Task<ResetPasswordToken?> GetAsync(ResetPasswordTokenId id);
    public Task<ResetPasswordToken> AddAsync(UserId userPasswordToResetId);
    public void Remove(ResetPasswordToken token);
}