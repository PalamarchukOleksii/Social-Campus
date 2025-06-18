using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.Publications.Queries.GetPublicationsForTag;

public record GetPublicationsForTagQuery(string TagLabel, int Page, int Count)
    : IQuery<IReadOnlyList<PublicationDto>>;