using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Users.GetAllRolesForUsers;
using TELERADIOLOGY.Domain.Entities;

public partial class GetAllUserRolesQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_UserRoles_OrderedByUserId()
    {
        
        var userRoles = new List<AppUserRole>
        {
            new AppUserRole { UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), RoleId = Guid.NewGuid() },
            new AppUserRole { UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), RoleId = Guid.NewGuid() }
        };

        var queryable = new TestAsyncEnumerable<AppUserRole>(userRoles);

        var mockRepo = new Mock<IUserRoleRepository>();
        mockRepo.Setup(repo => repo.GetAll()).Returns(queryable);

        var handler = new GetAllUserRolesQueryHandler(mockRepo.Object);
        var request = new GetAllUserRolesQuery();

       
        var result = await handler.Handle(request, CancellationToken.None);

      
        Assert.Equal(2, result.Count);
        Assert.True(result[0].UserId.CompareTo(result[1].UserId) < 0); 
    }

   
    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
        public TestAsyncEnumerable(Expression expression) : base(expression) { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }
}
