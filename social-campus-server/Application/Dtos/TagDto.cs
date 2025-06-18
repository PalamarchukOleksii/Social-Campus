using Domain.Models.TagModel;

namespace Application.Dtos;

public class TagDto
{
    public TagId Id { get; set; } = new(Guid.Empty);
    public string Label { get; set; } = string.Empty;
    public int PublicationsCount { get; set; }
}