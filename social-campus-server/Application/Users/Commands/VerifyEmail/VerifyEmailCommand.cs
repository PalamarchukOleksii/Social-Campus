using Application.Abstractions.Messaging;
using Domain.Models.EmailVerificationTokenModel;

namespace Application.Users.Commands.VerifyEmail;

public record VerifyEmailCommand(EmailVerificationTokenId EmailVerificationTokenId) : ICommand;