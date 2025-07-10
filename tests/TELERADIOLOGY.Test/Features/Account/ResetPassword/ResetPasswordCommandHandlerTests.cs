using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TELERADIOLOGY.Application.Features.AccountNotification.ResetPassword;
using TELERADIOLOGY.Domain.Entities;

public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly ResetPasswordCommandHandler _handler;

    public ResetPasswordCommandHandlerTests()
    {
        var store = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(
            store.Object,
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<AppUser>>(),
            Array.Empty<IUserValidator<AppUser>>(),
            Array.Empty<IPasswordValidator<AppUser>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<AppUser>>>()
        );
        _handler = new ResetPasswordCommandHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsFailure()
    {
        var command = new ResetPasswordCommand("unknown@example.com", "token", "NewPassword123!");
        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync((AppUser?)null);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Kullanıcı bulunamadı.");
    }

    [Fact]
    public async Task Handle_ResetFailed_ReturnsFailure()
    {
        var user = new AppUser { Email = "user@example.com" };
        var command = new ResetPasswordCommand(user.Email, "token", "NewPassword123!");
        var identityResult = IdentityResult.Failed(new IdentityError { Description = "Geçersiz token" });

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, command.Token, command.NewPassword)).ReturnsAsync(identityResult);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().Contain(x => x.Contains("Şifre sıfırlama başarısız"));
    }

    [Fact]
    public async Task Handle_ResetSucceeded_ReturnsSuccess()
    {
        var user = new AppUser { Email = "user@example.com" };
        var command = new ResetPasswordCommand(user.Email, "token", "NewPassword123!");

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, command.Token, command.NewPassword)).ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().Be("Şifreniz başarıyla sıfırlandı.");
    }
}
