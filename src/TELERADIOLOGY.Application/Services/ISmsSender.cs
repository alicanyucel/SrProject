namespace TELERADIOLOGY.Application.Services;

    public interface ISmsSender
    {
        Task SendSmsAsync(string toPhoneNumber, string message);
    }