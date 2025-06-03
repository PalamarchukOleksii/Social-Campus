using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.PublicationModel;

namespace Application.Publications.Queries.GetPublication;

public record GetPublicationQuery(PublicationId PublicationId) : IQuery<PublicationDto>;