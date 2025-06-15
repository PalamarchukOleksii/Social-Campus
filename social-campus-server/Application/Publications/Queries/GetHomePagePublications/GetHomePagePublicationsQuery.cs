using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Publications.Queries.GetHomePagePublications;

public record GetHomePagePublicationsQuery(UserId UserId, PublicationId? LastPublicationId, int Limit)
    : IQuery<IReadOnlyList<PublicationDto>>;