using FluentAssertions;
using GenericRepository;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Permissions.CreatePermission;
using TELERADIOLOGY.Domain.Entities;

public class CreatePermissionCommandHandlerTests
{
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreatePermissionCommandHandler _handler;

    public CreatePermissionCommandHandlerTests()
    {
        _permissionRepositoryMock = new Mock<IPermissionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreatePermissionCommandHandler(_permissionRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPermissionAlreadyExists()
    {
          
        var command = new CreatePermissionCommand("GET", "/api/users", "Kullanıcıları görüntüle");
        _permissionRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Permission, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.IsSuccessful.Should().BeFalse();  
        result.ErrorMessages.Should().ContainSingle().Which.Should().Be("izin zaten eklenmiş.");
        _permissionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Permission>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreatePermission_WhenPermissionDoesNotExist()
    {

        var command = new CreatePermissionCommand("/api/roles", "post", "Rol ekle");


        _permissionRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Permission, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Permission? capturedPermission = null;

        _permissionRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Permission>(), It.IsAny<CancellationToken>()))
            .Callback<Permission, CancellationToken>((perm, _) =>
            {
                capturedPermission = perm;
            })
            .Returns(Task.CompletedTask); // önemli!

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()));

       _unitOfWorkMock
           .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
           .ReturnsAsync(1); 

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _permissionRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Permission>(), It.IsAny<CancellationToken>()))
            .Callback<Permission, CancellationToken>((perm, _) =>
            {
                capturedPermission = perm;
            })
            .Returns(Task.CompletedTask);


        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); 
        var result = await _handler.Handle(command, CancellationToken.None);

       
        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().Be("İzinler oluşturuldu.");

        
        capturedPermission.Should().NotBeNull();
        capturedPermission.EndPoint.Should().Be("/api/roles");
        capturedPermission.Method.Should().Be("POST"); 
        capturedPermission.Description.Should().Be("Rol ekle");

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _permissionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Permission>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
