using Application.Abstractions.Messaging;
using Domain.Models.UserModel;
using Microsoft.AspNetCore.Http;

namespace Application.Publications.Commands.CreatePublication;

public record CreatePublicationCommand(string Description, UserId CreatorId, IFormFile? ImageData) : ICommand;