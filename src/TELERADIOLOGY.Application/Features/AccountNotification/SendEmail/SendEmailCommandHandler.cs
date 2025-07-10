using MediatR;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.AccontNotification.SendEmail;



public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Result<string>>
{
    private readonly EmailSettings _settings;

    public SendEmailCommandHandler(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<Result<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using var smtp = new SmtpClient(_settings.ServerAddress, _settings.ServerPort)
            {
                Credentials = new NetworkCredential(_settings.FromAddress, _settings.Password),
                EnableSsl = _settings.ServerUseSsl
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.FromAddress, _settings.FromName),
                Subject = request.Subject,
                Body = request.Message,
                IsBodyHtml = true
            };

            mail.To.Add(request.To);

            if (!string.IsNullOrEmpty(_settings.CcEmail))
                mail.CC.Add(_settings.CcEmail);

            if (!string.IsNullOrEmpty(_settings.BccEmail))
                mail.Bcc.Add(_settings.BccEmail);

            await smtp.SendMailAsync(mail, cancellationToken);

            return Result<string>.Succeed("E-posta başarıyla gönderildi.");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"E-posta gönderimi başarısız: {ex.Message}");
        }
    }
}
