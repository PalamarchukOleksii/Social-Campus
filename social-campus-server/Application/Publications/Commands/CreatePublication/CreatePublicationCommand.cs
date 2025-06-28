using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.UserModel;

namespace Application.Publications.Commands.CreatePublication;

public record CreatePublicationCommand(string Description, UserId CreatorId, FileUploadDto? ImageData) : ICommand;