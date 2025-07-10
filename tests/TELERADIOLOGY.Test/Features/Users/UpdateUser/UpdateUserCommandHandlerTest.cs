using AutoMapper;
using FluentAssertions;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TELERADIOLOGY.Application.Features.Users.UpdateUser;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;

public class UpdateUserByIdCommandHandlerTests
{
    private readonly Mock<RoleManager<AppRole>> _roleManagerMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly UpdateUserByIdCommandHandler _handler;

    public UpdateUserByIdCommandHandlerTests()
    {
        _roleManagerMock = new Mock<RoleManager<AppRole>>(
            new Mock<IRoleStore<AppRole>>().Object,
            new List<IRoleValidator<AppRole>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new Mock<ILogger<RoleManager<AppRole>>>().Object
        );

        _cacheServiceMock = new Mock<ICacheService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _userManagerMock = new Mock<UserManager<AppUser>>(
            new Mock<IUserStore<AppUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new PasswordHasher<AppUser>(),
            new List<IUserValidator<AppUser>>(),
            new List<IPasswordValidator<AppUser>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<AppUser>>>().Object
        );

        _mapperMock = new Mock<IMapper>();

        _handler = new UpdateUserByIdCommandHandler(
            _roleManagerMock.Object,
            _cacheServiceMock.Object,
            _unitOfWorkMock.Object,
            _userManagerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Should_Update_User_When_All_Data_Is_Valid()
    {
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var existingUser = new AppUser { Id = userId, UserName = "test@example.com" };
        var command = new UpdateUserByIdCommand(
            userId,
            Guid.Empty,
            "Updated",
            "LastName",
            "1234567890",
            "test@example.com",
            "NewPassword123!",
            "NewPassword123!",
            roleId,
            true,
            "12345678901",
            DateTime.UtcNow
        );

        // Ensure command.Password is not null before passing it to ResetPasswordAsync  
        if (string.IsNullOrEmpty(command.Password))
        {
            throw new ArgumentException("Password cannot be null or empty", nameof(command.Password));
        }

        _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(existingUser);

        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(existingUser))
            .ReturnsAsync("reset-token");

        _userManagerMock.Setup(x => x.ResetPasswordAsync(existingUser, "reset-token", command.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.GetRolesAsync(existingUser))
            .ReturnsAsync(new List<string> { "OldRole" });

        _userManagerMock.Setup(x => x.RemoveFromRolesAsync(existingUser, It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        _roleManagerMock.Setup(x => x.FindByIdAsync(command.RoleId.ToString()))
            .ReturnsAsync(new AppRole { Id = roleId, Name = "NewRole" });

        _userManagerMock.Setup(x => x.AddToRoleAsync(existingUser, "NewRole"))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.UpdateAsync(existingUser))
            .ReturnsAsync(IdentityResult.Success);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().Be("Kullanıcı başarıyla güncellendi");

        _cacheServiceMock.Verify(x => x.Remove("users"), Times.Once);
        _userManagerMock.Verify(x => x.UpdateAsync(existingUser), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Error_When_User_Not_Found()
    {
        var command = new UpdateUserByIdCommand(
            Guid.NewGuid(),
            Guid.Empty,
            "FirstName",
            "LastName",
            "1234567890",
            "test@example.com",
            null,
            null,
            Guid.NewGuid(),
            true,
            "12345678901",
            DateTime.UtcNow
        );

        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle().And.BeEquivalentTo("Kullanıcı bulunamadı");
    }

    [Fact]
    public async Task Should_Return_Error_When_Passwords_Do_Not_Match()
    {
        var user = new AppUser { Id = Guid.NewGuid() };

        var command = new UpdateUserByIdCommand(
            user.Id,
            Guid.Empty,
            "FirstName",
            "LastName",
            "1234567890",
            "test@example.com",
            "pass1",
            "pass2",
            Guid.NewGuid(),
            true,
            "12345678901",
            DateTime.UtcNow
        );

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Şifreler eşleşmiyor");
    }
}
