namespace Application.Dtos;

public class FileUploadDto
{
    public Stream Content { get; init; } = null!;
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
}