using Application.Abstractions.Email;
using Domain.Consts;
using Domain.Models.EmailVerificationTokenModel;
using Domain.Models.ResetPasswordTokenModel;
using Domain.Models.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Email;

public class EmailLinkFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    : IEmailLinkFactory
{
    public string? CreateEmailVerificationLink(Guid generatedToken, string userEmail)
    {
        var verificationLink =
            linkGenerator.GetUriByName(
                httpContextAccessor.HttpContext!,
                LinkConsts.VerifyEmail,
                new { token = generatedToken, email = userEmail });

        return verificationLink;
    }

    public string? CreateResetPasswordLink(Guid generatedToken, UserId userId)
    {
        var resetLink =
            linkGenerator.GetUriByName(
                httpContextAccessor.HttpContext!,
                LinkConsts.ResetPassword,
                new { token = generatedToken, userId = userId.Value });

        return resetLink;
    }
}