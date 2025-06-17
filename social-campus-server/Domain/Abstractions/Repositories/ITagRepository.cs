using Domain.Models.TagModel;

namespace Domain.Abstractions.Repositories;

public interface ITagRepository
{
    public Task AddAsync(Tag tag, CancellationToken cancellationToken = default);
    public Task<bool> ExistsAsync(string label, CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<Tag>> SearchAsync(string searchTerm, int page, int count);
}