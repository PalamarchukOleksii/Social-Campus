namespace Application.Abstractions.Email;

public interface IEmailService
{
    public Task SendEmailAsync(string messageReceiver, string messageSubject, string messageBody, bool isHtml);
}