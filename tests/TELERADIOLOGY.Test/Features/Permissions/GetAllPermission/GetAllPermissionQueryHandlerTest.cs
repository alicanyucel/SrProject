using FluentAssertions;
using Moq;
using TELERADIOLOGY.Application.Dtos.Permission;
using TELERADIOLOGY.Application.Features.Permissions.GetAllPermission;
using TELERADIOLOGY.Domain.Entities;

public class GetAllPermissionsQueryHandlerTests
{
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
    private readonly GetAllPermissionsQueryHandler _handler;

    public GetAllPermissionsQueryHandlerTests()
    {
        _permissionRepositoryMock = new Mock<IPermissionRepository>();
        _handler = new GetAllPermissionsQueryHandler(_permissionRepositoryMock.Object);
    }

    [Fact(DisplayName = "Handle: silinmemiş izinleri döner")]
    public async Task Handle_ShouldReturnOnlyNonDeletedPermissions()
    {
       
        var data = new List<Permission>
        {
            new Permission { Id = 1, EndPoint = "/api/a", Method = "GET", Description = "A", IsDeleted = false },
            new Permission { Id = 2, EndPoint = "/api/b", Method = "POST", Description = "B", IsDeleted = true },
            new Permission { Id = 3, EndPoint = "/api/c", Method = "PUT", Description = "C", IsDeleted = false },
        }.AsQueryable();

        _permissionRepositoryMock
            .Setup(r => r.GetAll())
            .Returns(data);

        var result = await _handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);
        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().BeEquivalentTo(new[]
        {
            new PermissionDto { Id = 1, EndPoint = "/api/a", Method = "GET", Description = "A" },
            new PermissionDto { Id = 3, EndPoint = "/api/c", Method = "PUT", Description = "C" }
        }, options => options.WithStrictOrdering());
    }
    [Fact(DisplayName = "Handle: hiç izin yoksa boş liste döner")]
    public async Task Handle_ShouldReturnEmptyList_WhenNoPermissions()
    {
        
        _permissionRepositoryMock
            .Setup(r => r.GetAll())
            .Returns(Enumerable.Empty<Permission>().AsQueryable());
        var result = await _handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);
        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }
}
