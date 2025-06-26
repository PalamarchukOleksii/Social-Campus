using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Application.Dtos;
using Application.Helpers;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.Login;

public class LoginCommandHandler(
    IJwtProvider jwtProvider,
    IUserRepository userRepository,
    IRefreshTokenRepository tokenRepository,
    IHasher hasher,
    IEmailService emailService,
    IEmailVerificationTokenRepository emailVerificationTokenRepository,
    IEmailLinkFactory emailLinkFactory) : ICommandHandler<LoginCommand, UserLoginRefreshDto>
{
    public async Task<Result<UserLoginRefreshDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            return Result.Failure<UserLoginRefreshDto>(new Error(
                "User.NotFound",
                $"User with email {request.Email} was not found"));

        var isPasswordValid = await hasher.VerifyAsync(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            return Result.Failure<UserLoginRefreshDto>(new Error(
                "User.IncorrectPassword",
                "Incorrect password"));

        if (!user.IsEmailVerified)
        {
            var emailVerificationToken = await emailVerificationTokenRepository.AddAsync(user.Id);
            var verificationLink = emailLinkFactory.CreateEmailVerificationLink(emailVerificationToken.Id);
            if (verificationLink is null)
                return Result.Failure<UserLoginRefreshDto>(new Error(
                    "Email.LinkGenerationFailed",
                    "Unable to generate email verification link"));

            var messageBody = EmailTemplateHelpers.GetVerifyEmailHtml(user.FirstName, verificationLink);
            await emailService.SendEmailAsync(user.Email, "Resend Email Verification", messageBody, true);

            return Result.Failure<UserLoginRefreshDto>(new Error(
                "User.EmailNotVerified",
                "Your email is not verified. A new verification link has been sent."));
        }

        var tokens = jwtProvider.GenerateTokens(user);
        var existingRefreshToken = user.RefreshTokenId.Value != Guid.Empty
            ? await tokenRepository.GetByIdAsync(user.RefreshTokenId)
            : null;

        if (existingRefreshToken != null)
        {
            tokenRepository.Update(existingRefreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInSeconds);
        }
        else
        {
            var refreshToken =
                await tokenRepository.AddAsync(tokens.RefreshToken, tokens.RefreshTokenExpirationInSeconds, user.Id);
            user.SetRefreshTokenId(refreshToken.Id);
        }

        return Result.Success(new UserLoginRefreshDto
        {
            Tokens = tokens,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Login = user.Login,
            ProfileImageUrl = user.ProfileImageUrl
        });
    }
}