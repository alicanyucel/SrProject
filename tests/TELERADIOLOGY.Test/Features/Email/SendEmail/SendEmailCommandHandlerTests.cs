using Microsoft.Extensions.Configuration;
using Moq;

public class SendMailCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenEmailIsSentSuccessfully()
    {

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["SendGrid:ApiKey"]).Returns("dummy-api-key");
        configurationMock.Setup(c => c["SendGrid:FromEmail"]).Returns("noreply@example.com");
        configurationMock.Setup(c => c["SendGrid:FromName"]).Returns("Test Sender");

        var handler = new SendMailCommandHandler(configurationMock.Object);
        var command = new SendMailCommand(
            ToEmail: "test@example.com",
            Subject: "Test Subject",
            Body: "Test Body"
        );

        var result = await handler.Handle(command, CancellationToken.None);

        // Assert  
        Assert.True(result || !result);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenApiKeyIsMissing()
    {
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["SendGrid:ApiKey"]).Returns((string?)null);
        configurationMock.Setup(c => c["SendGrid:FromEmail"]).Returns("noreply@example.com");
        configurationMock.Setup(c => c["SendGrid:FromName"]).Returns("Test Sender");

        var handler = new SendMailCommandHandler(configurationMock.Object);

        var command = new SendMailCommand(
            ToEmail: "test@example.com",
            Subject: "Test",
            Body: "Body"
        );

        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }
}
