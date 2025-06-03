using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.UserModel;

namespace Application.Users.Queries.GetUserPublications;

public record GetUserPublicationsQuery(UserId UserId) : IQuery<IReadOnlyList<PublicationDto>>;