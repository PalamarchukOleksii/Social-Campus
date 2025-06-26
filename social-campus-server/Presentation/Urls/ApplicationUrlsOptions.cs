namespace Presentation.Urls;

public class ApplicationUrlsOptions
{
    public const string SectionName = "ApplicationUrls";
    public FrontendUrls Frontend { get; init; } = new();
    public CorsUrls Cors { get; init; } = new();
}