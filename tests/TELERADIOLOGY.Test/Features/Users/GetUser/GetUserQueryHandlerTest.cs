using Microsoft.EntityFrameworkCore;
using Moq;
using TELERADIOLOGY.Application.Features.Users.GetUserById;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetUserQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object);
    }
    [Fact]
    public async Task Should_Throw_When_User_Not_Found()
    {
        var userId = Guid.NewGuid();
        var query = new GetUserByIdQuery(userId); 
        var users = new List<AppUser>().AsQueryable();
        var mockDbSet = new Mock<DbSet<AppUser>>();
        mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(users.Provider);
        mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(users.Expression);
        mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
        _userRepositoryMock.Setup(r => r.GetAll()).Returns(mockDbSet.Object);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
