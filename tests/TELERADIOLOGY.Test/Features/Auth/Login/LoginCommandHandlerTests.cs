using GenericRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using TELERADIOLOGY.Application.Features.Auth.Login;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

public class LoginCommandHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILoginRepository> _loginRepositoryMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly LoginCommandHandler _handler;
    public LoginCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(
            userStoreMock.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<AppUser>>().Object,
            Array.Empty<IUserValidator<AppUser>>(),
            Array.Empty<IPasswordValidator<AppUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<AppUser>>>().Object
        );
        var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
        _signInManagerMock = new Mock<SignInManager<AppUser>>(
            _userManagerMock.Object,
            contextAccessorMock.Object,
            claimsFactoryMock.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<AppUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<AppUser>>().Object
        );

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loginRepositoryMock = new Mock<ILoginRepository>();
        _jwtProviderMock = new Mock<IJwtProvider>();

        _handler = new LoginCommandHandler(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _unitOfWorkMock.Object,
            _loginRepositoryMock.Object,
            _jwtProviderMock.Object
        );
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsError()
    {
      
        var request = new LoginCommand("nonexistent", "password");
        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<AppUser>().AsQueryable().BuildMockDbSet().Object);

       
        var result = await _handler.Handle(request, default);

       
        Assert.False(result.IsSuccessful);
        Assert.NotNull(result.ErrorMessages); 
        Assert.Contains("Kullanıcı bulunamadı", result.ErrorMessages);
    }

    [Fact]
    public async Task Handle_WrongPassword_ReturnsError()
    {
        var user = new AppUser { UserName = "test", Email = "test@example.com" };
        var request = new LoginCommand("test", "wrongpassword");
        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<AppUser> { user }.AsQueryable().BuildMockDbSet().Object);
        _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, request.Password, true))
            .ReturnsAsync(SignInResult.Failed);
        var result = await _handler.Handle(request, default);
        Assert.False(result.IsSuccessful);
        Assert.NotNull(result.ErrorMessages); 
        Assert.Contains("Şifreniz yanlış", result.ErrorMessages);   
    }

    private void ReturnsAsync(Result<LoginCommandResponse> result)
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task Handle_LoginNotAllowed_ReturnsNotAllowedMessage()
    {
        var user = new AppUser { UserName = "test", Email = "test@example.com" };
        var request = new LoginCommand("test", "any");
        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<AppUser> { user }.AsQueryable().BuildMockDbSet().Object);

        _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, request.Password, true))
            .ReturnsAsync(SignInResult.NotAllowed);
        var result = await _handler.Handle(request, default);
        Assert.False(result.IsSuccessful);
        Assert.NotNull(result.ErrorMessages); 
        Assert.Contains("Mail adresiniz onaylı değil", result.ErrorMessages!); 
    }

    [Fact]
    public async Task Handle_SuccessfulLogin_ReturnsToken()
    {
        var user = new AppUser { UserName = "test", Email = "test@example.com", LoginId = Guid.NewGuid() };
        var request = new LoginCommand("test", "correctpassword");
        _userManagerMock.Setup(um => um.Users)
            .Returns(new List<AppUser> { user }.AsQueryable().BuildMockDbSet().Object);
        _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, request.Password, true))
            .ReturnsAsync(SignInResult.Success);
        _loginRepositoryMock.Setup(repo => repo.GetByExpressionWithTrackingAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Login, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Login());
        _jwtProviderMock.Setup(j => j.CreateToken(user))
            .ReturnsAsync(new LoginCommandResponse(
                "fake.jwt.token",
                "fake.refresh.token",
                DateTime.UtcNow.AddDays(7)
            ));
        var result = await _handler.Handle(request, default);
        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Data);
        Assert.Equal("fake.jwt.token", result.Data.Token);
    }
}
