using Application.Abstractions.Messaging;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Microsoft.AspNetCore.Http;

namespace Application.Publications.Commands.UpdatePublication;

public record UpdatePublicationCommand(
    UserId CallerId,
    PublicationId PublicationId,
    string Description,
    IFormFile? ImageData) : ICommand;