﻿using FluentValidation;

namespace Application.RefreshTokens.Commands.RefreshCommand
{
    public class RefreshCommandValidator : AbstractValidator<RefreshCommandRequest>
    {
        public RefreshCommandValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Access token is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MinimumLength(128).WithMessage("Refresh token format is invalid.");
        }
    }
}
