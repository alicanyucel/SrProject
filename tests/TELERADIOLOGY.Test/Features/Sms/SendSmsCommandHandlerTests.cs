using Moq;
using TELERADIOLOGY.Application.Features.SendSms;
using TELERADIOLOGY.Application.Features.Sms;
using TELERADIOLOGY.Application.Services;

public class SendSmsCommandHandlerTests
{
    private readonly Mock<ISmsSender> _smsSenderMock;
    private readonly SendSmsCommandHandler _handler;

    public SendSmsCommandHandlerTests()
    {
        _smsSenderMock = new Mock<ISmsSender>();
        _handler = new SendSmsCommandHandler(_smsSenderMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenSmsIsSent()
    {
        var command = new SendSmsCommand("5551234567", "Test mesajı");

        _smsSenderMock
            .Setup(s => s.SendSmsAsync(command.PhoneNumber, command.Message))
            .Returns(Task.CompletedTask);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result.IsSuccessful);
        Assert.Equal("SMS başarıyla gönderildi.", result.Data);
        _smsSenderMock.Verify(s => s.SendSmsAsync(command.PhoneNumber, command.Message), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSmsSendingFails()
    {
        var command = new SendSmsCommand("5551234567", "Test mesajı");
        _smsSenderMock
            .Setup(s => s.SendSmsAsync(command.PhoneNumber, command.Message))
            .ThrowsAsync(new InvalidOperationException("Servis hatası"));
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.False(result.IsSuccessful);
        Assert.Contains("SMS gönderilemedi", result.ErrorMessages![0]);
        _smsSenderMock.Verify(s => s.SendSmsAsync(command.PhoneNumber, command.Message), Times.Once);
    }
}
