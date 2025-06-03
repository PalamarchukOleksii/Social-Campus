using Application.Abstractions.Messaging;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Publications.Commands.UpdatePublication;

public record UpdatePublicationCommand(
    UserId CallerId,
    PublicationId PublicationId,
    string Description,
    string ImageData) : ICommand;