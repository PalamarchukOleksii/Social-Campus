using Application.Abstractions.Messaging;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Publications.Commands.DeletePublication;

public record DeletePublicationCommand(PublicationId PublicationId, UserId CallerId) : ICommand;