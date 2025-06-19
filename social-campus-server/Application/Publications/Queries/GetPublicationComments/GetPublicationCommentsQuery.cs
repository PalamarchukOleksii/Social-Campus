using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.PublicationModel;

namespace Application.Publications.Queries.GetPublicationComments;

public record GetPublicationCommentsQuery(PublicationId PublicationId, int Page, int Count)
    : IQuery<IReadOnlyList<CommentDto>>;