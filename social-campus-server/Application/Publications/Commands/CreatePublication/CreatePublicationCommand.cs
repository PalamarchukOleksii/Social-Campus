using Application.Abstractions.Messaging;
using Domain.Models.UserModel;

namespace Application.Publications.Commands.CreatePublication;

public record CreatePublicationCommand(string Description, UserId CreatorId, string ImageData) : ICommand;