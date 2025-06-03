using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.UserModel;

namespace Application.Publications.Queries.GetUserPublications;

public record GetUserPublicationsQuery(UserId UserId) : IQuery<IReadOnlyList<PublicationDto>>;