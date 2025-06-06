using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.PublicationModel;

namespace Application.Publications.Queries.GetPublicationComments;

public record GetPublicationCommentsQuery(PublicationId PublicationId) : IQuery<IReadOnlyList<CommentDto>>;