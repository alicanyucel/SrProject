using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TELERADIOLOGY.Application.Features.AccountNotification.ForgotPassword;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Services.NetMail;

namespace TELERADIOLOGY.Test.Features.Account.ForgotPassword;
public class ForgotPasswordCommandHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly ForgotPasswordCommandHandler _handler;

    public ForgotPasswordCommandHandlerTests()
    {
        var store = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(
            store.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<AppUser>>().Object,
            new IUserValidator<AppUser>[0],
            new IPasswordValidator<AppUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<AppUser>>>().Object
        );
        _emailSenderMock = new Mock<IEmailSender>();
        _handler = new ForgotPasswordCommandHandler(_userManagerMock.Object, _emailSenderMock.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsFailure()
    {
        var command = new ForgotPasswordCommand("nonexistent@example.com");
        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync((AppUser?)null);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Kullanıcı bulunamadı.");
    }

    [Fact]
    public async Task Handle_UserFound_SendsEmail_ReturnsSuccess()
    {
        var user = new AppUser { Email = "test@example.com" };
        var command = new ForgotPasswordCommand(user.Email);
        var token = "mock-token";

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(token);
        _emailSenderMock.Setup(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().Be(token);
        _emailSenderMock.Verify(x => x.SendEmailAsync(
            user.Email,
            "Şifre Sıfırlama",
            It.Is<string>(s => s.Contains("reset-password") && s.Contains("token="))
        ), Times.Once);
    }
}
