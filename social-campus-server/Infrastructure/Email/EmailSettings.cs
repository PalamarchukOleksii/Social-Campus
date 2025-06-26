namespace Infrastructure.Email;

public class EmailSettings
{
    public string SmtpHost { get; init; } = string.Empty;
    public int SmtpPort { get; init; }
    public string SmtpUser { get; init; } = string.Empty;
    public string SmtpPass { get; init; } = string.Empty;
}