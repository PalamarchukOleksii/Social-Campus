namespace Infrastructure.Options;

public class EmailOptions
{
    public string SmtpHost { get; init; } = string.Empty;
    public int SmtpPort { get; init; }
    public string SmtpUser { get; init; } = string.Empty;
    public string SmtpPass { get; init; } = string.Empty;
    public bool EnableSsl { get; set; }
}