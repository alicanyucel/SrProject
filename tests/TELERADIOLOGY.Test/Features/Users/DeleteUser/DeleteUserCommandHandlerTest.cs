using FluentAssertions;
using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Users.DeleteUser;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class DeleteUserByIdCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteUserByIdCommandHandler _handler;

    public DeleteUserByIdCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new DeleteUserByIdCommandHandler(
            _userRepositoryMock.Object,
            _cacheServiceMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Should_Delete_User_When_User_Exists()
    {
        var userId = Guid.NewGuid();
        var user = new AppUser { Id = userId, IsDeleted = false };

        _userRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<AppUser, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var command = new DeleteUserByIdCommand(userId);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().Be("Kullanıcı başarıyla silindi.");

        user.IsDeleted.Should().BeTrue();
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(x => x.Remove("users"), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Error_When_User_Not_Found()
    {
        _userRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<AppUser, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((AppUser?)null); // Fix: Explicitly mark the null value as nullable  

        var command = new DeleteUserByIdCommand(Guid.NewGuid());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Kullanıcı bulunamadı."); // Fix: Use `ContainSingle` for collection assertions  

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(x => x.Remove("users"), Times.Never);
    }
}

