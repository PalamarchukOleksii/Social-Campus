using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Tags.Queries.SearchTags;

public class SearchTagsQueryHandler(
    ITagRepository tagRepository,
    IPublicationTagRepository publicationTagRepository)
    : IQueryHandler<SearchTagsQuery, IReadOnlyList<TagDto>>
{
    public async Task<Result<IReadOnlyList<TagDto>>> Handle(SearchTagsQuery request,
        CancellationToken cancellationToken)
    {
        var tags = await tagRepository.SearchAsync(request.SearchTerm, request.Page, request.Count);

        List<TagDto> tagDtos = [];
        foreach (var tag in tags)
        {
            var publications = await publicationTagRepository.GetAllPublicationsForTagAsync(tag.Id, cancellationToken);

            tagDtos.Add(new TagDto
            {
                Id = tag.Id,
                Label = tag.Label,
                PublicationsCount = publications.Count
            });
        }

        return Result.Success<IReadOnlyList<TagDto>>(tagDtos);
    }
}