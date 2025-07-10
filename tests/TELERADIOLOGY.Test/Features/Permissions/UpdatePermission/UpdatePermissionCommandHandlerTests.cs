using FluentAssertions;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Permissions.UpdatePermission;
using TELERADIOLOGY.Domain.Entities;

public class UpdatePermissionCommandHandlerTests
{
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdatePermissionCommandHandler _handler;

    public UpdatePermissionCommandHandlerTests()
    {
        _permissionRepositoryMock = new Mock<IPermissionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdatePermissionCommandHandler(
            _permissionRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }


    private bool ExprMatchesId(Expression<Func<Permission, bool>> expr, int expectedId)
    {
        if (expr.Body is BinaryExpression bin &&
            bin.NodeType == ExpressionType.Equal &&
            bin.Left is MethodCallExpression mce &&
            mce.Method.Name == nameof(EF.Property) &&
            bin.Right is ConstantExpression ce)
        {
            if (ce.Value is int id) return id == expectedId;
        }
        return false;
    }

    [Fact(DisplayName = "Handle: bulunamayan izin için Failure dönmeli")]
    public async Task Handle_ShouldReturnFailure_WhenPermissionNotFound()
    {
        var id = 123;

        var command = new UpdatePermissionCommand(id, "/api/new", "put", "new desc");

        _permissionRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Permission?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle().Which.Should().Be("İzin bulunamadı.");
        _permissionRepositoryMock.Verify(r => r.Update(It.IsAny<Permission>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}