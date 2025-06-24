using Application.Abstractions.Email;
using Domain.Consts;
using Domain.Models.EmailVerificationTokenModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Email;

public class EmailVerificationLinkFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    : IEmailVerificationLinkFactory
{
    public string? Create(EmailVerificationTokenId emailVerificationTokenId)
    {
        var verificationLink =
            linkGenerator.GetUriByName(
                httpContextAccessor.HttpContext!,
                VerifyConsts.VerifyEmail,
                new { token = emailVerificationTokenId.Value });

        return verificationLink;
    }
}