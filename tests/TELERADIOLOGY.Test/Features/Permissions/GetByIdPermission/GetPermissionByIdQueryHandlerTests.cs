using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Dtos.Permission;
using TELERADIOLOGY.Application.Features.Permissions.GetByIdPermission;
using TELERADIOLOGY.Domain.Entities;

public class GetPermissionByIdQueryHandlerTests
{
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
    private readonly GetPermissionByIdQueryHandler _handler;

    public GetPermissionByIdQueryHandlerTests()
    {
        _permissionRepositoryMock = new Mock<IPermissionRepository>();
        _handler = new GetPermissionByIdQueryHandler(_permissionRepositoryMock.Object);
    }

    [Fact(DisplayName = "Handle: id'ye ait izin bulunamazsa failure dönmeli")]
    public async Task Handle_ShouldReturnFailure_WhenPermissionNotFound()
    {
       
        var query = new GetPermissionByIdQuery(42);
        _permissionRepositoryMock
            .Setup(r => r.GetByExpressionAsync(
                It.IsAny<Expression<Func<Permission, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Permission?)null);
        var result = await _handler.Handle(query, CancellationToken.None);
        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle().Which.Should().Be("İzin bulunamadı.");
    }

    [Fact(DisplayName = "Handle: id'ye ait izin varsa doğru DTO ile success dönmeli")]
    public async Task Handle_ShouldReturnDto_WhenPermissionExists()
    {
       
        var permission = new Permission
        {
            Id = 7,
            EndPoint = "/api/test",
            Method = "POST",
            Description = "Test izin"
        };
        _permissionRepositoryMock
            .Setup(r => r.GetByExpressionAsync(
                It.Is<Expression<Func<Permission, bool>>>(expr =>
                    expr.Compile().Invoke(permission)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        var query = new GetPermissionByIdQuery(permission.Id);
        var result = await _handler.Handle(query, CancellationToken.None);
        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(new PermissionDto
        {
            Id = permission.Id,
            EndPoint = permission.EndPoint,
            Method = permission.Method,
            Description = permission.Description
        });
    }
}
