using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.TagModel;

namespace Application.Tags.Queries.SearchTags;

public record SearchTagsQuery(string SearchTerm, int Page, int Count) : IQuery<IReadOnlyList<TagDto>>;