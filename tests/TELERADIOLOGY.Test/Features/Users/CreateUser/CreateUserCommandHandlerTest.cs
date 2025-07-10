using FluentAssertions;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TELERADIOLOGY.Application.Features.Users.Createuser;
using TELERADIOLOGY.Application.Features.Users.CreateUser;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<ILoginRepository> _loginRepositoryMock;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher<Login>> _passwordHasherMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _loginRepositoryMock = new Mock<ILoginRepository>();
        _userManagerMock = MockUserManager();
        _cacheServiceMock = new Mock<ICacheService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _passwordHasherMock = new Mock<IPasswordHasher<Login>>();

        _handler = new CreateUserCommandHandler(
            _loginRepositoryMock.Object,
            _userManagerMock.Object,
            _cacheServiceMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Should_Create_User_When_Passwords_Match()
    {
        var command = new CreateUserCommand(
            FirstName: "John",
            LastName: "Doe",
            Email: "john@example.com",
            PhoneNumber: "1234567890",
            Password: "Password123!",
            Repassword: "Password123!",
            IdentityNumber: "12345678901",
            RoleId: Guid.NewGuid(),
            IsActive: true
        );

        _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<Login>(), command.Password))
            .Returns("hashed_password");

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().Be("Kullanıcı eklendi.");

        _loginRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Login>(), It.IsAny<CancellationToken>()), Times.Once);
        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(x => x.Remove("users"), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Error_When_Passwords_Do_Not_Match()
    {
        var command = new CreateUserCommand(
            FirstName: "Jane",
            LastName: "Smith",
            Email: "jane@example.com",
            PhoneNumber: "0987654321",
            Password: "Password1!",
            Repassword: "Password2!",
            IdentityNumber: "10987654321",
            RoleId: Guid.NewGuid(),
            IsActive: true
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.StatusCode.Should().Be(400);
        result.ErrorMessages.Should().ContainSingle("Şifreler eşleşmiyor.");

        _loginRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Login>(), It.IsAny<CancellationToken>()), Times.Never);
        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Never);
    }

    private static Mock<UserManager<AppUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<AppUser>>();
        return new Mock<UserManager<AppUser>>(
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
    }
}
