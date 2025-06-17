using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.UserModel;

namespace Application.Publications.Queries.GetHomePagePublications;

public record GetHomePagePublicationsQuery(UserId UserId, int Page, int Count)
    : IQuery<IReadOnlyList<PublicationDto>>;