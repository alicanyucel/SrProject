using System.Net.Mail;

namespace TELERADIOLOGY.Domain.Services.NetMail
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailWithAttachmentsAsync(string email, string subject, string message, List<Attachment> attachments);
    }
}
