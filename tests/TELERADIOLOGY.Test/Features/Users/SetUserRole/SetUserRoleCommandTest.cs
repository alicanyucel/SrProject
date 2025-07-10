using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Roles.UserRoles.Commands;
using TELERADIOLOGY.Domain.Entities;

public class SetUserRoleCommandHandlerTests
{
    private readonly Mock<IUserRoleRepository> _roleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly SetUserRoleCommandHandler _handler;

    public SetUserRoleCommandHandlerTests()
    {
        _roleRepositoryMock = new Mock<IUserRoleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new SetUserRoleCommandHandler(_roleRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_AddUserRole_When_Not_Already_Assigned()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var command = new SetUserRoleCommand(userId, roleId);

        _roleRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<AppUserRole, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _roleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<AppUserRole>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); // Burada async int dönüyoruz!

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _roleRepositoryMock.Verify(r => r.AnyAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<AppUserRole, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);

        _roleRepositoryMock.Verify(r => r.AddAsync(
            It.Is<AppUserRole>(ur => ur.UserId == userId && ur.RoleId == roleId), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(MediatR.Unit.Value, result);
    }


    [Fact]
    public async Task Handle_Should_Throw_When_UserRole_Already_Assigned()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var command = new SetUserRoleCommand(userId, roleId);

        _roleRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<AppUserRole, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
