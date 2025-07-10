using FluentAssertions;
using GenericRepository;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Permissions.DeletePermission;
using TELERADIOLOGY.Domain.Entities;

public class DeletePermissionByIdCommandHandlerTests
{
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeletePermissionByIdCommandHandler _handler;

    public DeletePermissionByIdCommandHandlerTests()
    {
        _permissionRepositoryMock = new Mock<IPermissionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeletePermissionByIdCommandHandler(_permissionRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    private bool CheckExpressionMatchesId(Expression<Func<Permission, bool>> expr, int expectedId)
    {
        if (expr.Body is BinaryExpression binaryExpr)
        {
            if (binaryExpr.Right is ConstantExpression constantExpr)
            {
                return constantExpr.Value?.ToString() == expectedId.ToString();
            }
        }
        return false;
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPermissionNotFound()
    {
        var command = new DeletePermissionByIdCommand(1);

        _permissionRepositoryMock
            .Setup(x => x.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Permission?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle().Which.Should().Be("İzin zaten silinmiş."); 
        _permissionRepositoryMock.Verify(x => x.Update(It.IsAny<Permission>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPermissionAlreadyDeleted()
    {
        var permission = new Permission { Id = 1, IsDeleted = true };

        _permissionRepositoryMock
            .Setup(x => x.GetByExpressionWithTrackingAsync(
                It.Is<Expression<Func<Permission, bool>>>(expr => CheckExpressionMatchesId(expr, permission.Id)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        var result = await _handler.Handle(new DeletePermissionByIdCommand(permission.Id), CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle().Which.Should().Be("İzin zaten silinmiş.");
        _permissionRepositoryMock.Verify(x => x.Update(It.IsAny<Permission>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
  
}
