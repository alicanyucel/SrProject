using Microsoft.EntityFrameworkCore;
using Moq;
using TELERADIOLOGY.Application.Features.Members.GetByIdMember;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetMemberByIdQueryHandlerTests
{
   
    [Fact]
    public async Task Handle_Should_ThrowException_When_MemberNotFound()
    {
        var memberId = Guid.NewGuid();
        var members = new List<Member>().AsQueryable(); // Empty list
        var mockDbSet = new Mock<DbSet<Member>>();
        mockDbSet.As<IQueryable<Member>>().Setup(m => m.Provider).Returns(members.Provider);
        mockDbSet.As<IQueryable<Member>>().Setup(m => m.Expression).Returns(members.Expression);
        mockDbSet.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns(members.ElementType);
        mockDbSet.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns(() => members.GetEnumerator());

        var memberRepositoryMock = new Mock<IMemberRepository>();
        memberRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        var handler = new GetMemberByIdQueryHandler(memberRepositoryMock.Object);
        var query = new GetMemberByIdQuery(memberId);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
        handler.Handle(query, CancellationToken.None));
    }
}