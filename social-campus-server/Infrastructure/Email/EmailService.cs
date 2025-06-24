using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Application.Abstractions.Email;
using Microsoft.Extensions.Options;

namespace Infrastructure.Email;

public class EmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly string _smtpHost = options.Value.SmtpHost;
    private readonly int _smtpPort = options.Value.SmtpPort;
    private readonly string _smtpUser = options.Value.SmtpUser;
    private readonly string _smtpPass = options.Value.SmtpPass;

    public async Task SendEmailAsync(string messageReceiver, string messageSubject, string messageBody)
    {
        using var mail = new MailMessage();
        mail.From = new MailAddress(string.IsNullOrEmpty(_smtpUser) ? "test@local.test" : _smtpUser);
        mail.To.Add(messageReceiver);
        mail.Subject = messageSubject;
        mail.Body = messageBody;

        using var smtp = new SmtpClient(_smtpHost, _smtpPort);

        if (!string.IsNullOrEmpty(_smtpUser) && !string.IsNullOrEmpty(_smtpPass))
        {
            smtp.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
            smtp.EnableSsl = true;
        }
        else
        {
            smtp.EnableSsl = false;
        }

        await smtp.SendMailAsync(mail);
    }
}