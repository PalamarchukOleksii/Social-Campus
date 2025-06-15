using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Users.Queries.GetUserPublications;

public record GetUserPublicationsQuery(UserId UserId, PublicationId? LastPublicationId, int Count)
    : IQuery<IReadOnlyList<PublicationDto>>;