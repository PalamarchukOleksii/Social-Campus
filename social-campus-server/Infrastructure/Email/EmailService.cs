using System.Net;
using System.Net.Mail;
using System.Text;
using Application.Abstractions.Email;
using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Email;

public class EmailService(IOptions<EmailOptions> options) : IEmailService
{
    private readonly bool _enableSsl = options.Value.EnableSsl;
    private readonly string _smtpHost = options.Value.SmtpHost;
    private readonly string _smtpPass = options.Value.SmtpPass;
    private readonly int _smtpPort = options.Value.SmtpPort;
    private readonly string _smtpUser = options.Value.SmtpUser;

    public async Task SendEmailAsync(string messageReceiver, string messageSubject, string messageBody, bool isHtml)
    {
        using var mail = new MailMessage();
        mail.From = new MailAddress(string.IsNullOrEmpty(_smtpUser) ? "test@local.test" : _smtpUser);
        mail.Subject = messageSubject;
        mail.Body = messageBody;
        mail.IsBodyHtml = isHtml;
        mail.BodyEncoding = Encoding.UTF8;
        mail.SubjectEncoding = Encoding.UTF8;

        mail.To.Add(messageReceiver);

        using var smtp = new SmtpClient(_smtpHost, _smtpPort);
        smtp.EnableSsl = _enableSsl;

        if (!string.IsNullOrEmpty(_smtpUser) && !string.IsNullOrEmpty(_smtpPass))
            smtp.Credentials = new NetworkCredential(_smtpUser, _smtpPass);

        await smtp.SendMailAsync(mail);
    }
}