using MediatR;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

public class SendMailCommandHandler : IRequestHandler<SendMailCommand, bool>
{
    private readonly IConfiguration _configuration;

    public SendMailCommandHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> Handle(SendMailCommand request, CancellationToken cancellationToken)
    {
        var apiKey = _configuration["SendGrid:ApiKey"];
        var fromEmail = _configuration["SendGrid:FromEmail"];
        var fromName = _configuration["SendGrid:FromName"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(fromEmail, fromName);
        var to = new EmailAddress(request.ToEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, request.Subject, request.Body, request.Body);
        var response = await client.SendEmailAsync(msg);

        return response.IsSuccessStatusCode;
    }

}
