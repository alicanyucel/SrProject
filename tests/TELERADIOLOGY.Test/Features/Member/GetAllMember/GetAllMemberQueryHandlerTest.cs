using MockQueryable;
using Moq;
using TELERADIOLOGY.Application.Features.Members.GetAllMember;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetAllMemberQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsMembersFromCache_IfCacheIsNotNull()
    {
        var mockRepo = new Mock<IMemberRepository>();
        var mockCache = new Mock<ICacheService>();

        var cachedMembers = new List<Member> { new Member { FirstName = "Ali" } };
        mockCache.Setup(c => c.Get<List<Member>>("members")).Returns(cachedMembers);

        var handler = new GetAllMemberQueryHandler(mockRepo.Object, mockCache.Object);

        var result = await handler.Handle(new GetAllMemberQuery(), CancellationToken.None);

        Assert.Equal(cachedMembers, result.Data); // Fix: Changed 'Value' to 'Data'  
        mockRepo.Verify(r => r.GetAll(), Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsMembersFromRepository_IfCacheIsNull()
    {
        var mockRepo = new Mock<IMemberRepository>();
        var mockCache = new Mock<ICacheService>();

        mockCache.Setup(c => c.Get<List<Member>>("members")).Returns((List<Member>?)null);

        var membersFromRepo = new List<Member>
        {
            new Member { FirstName = "Zeynep" },
            new Member { FirstName = "Ahmet" }
        };

        // MockQueryable ile async LINQ desteği  
        var mockQueryable = membersFromRepo.AsQueryable().BuildMock();

        mockRepo.Setup(r => r.GetAll()).Returns(mockQueryable);

        var handler = new GetAllMemberQueryHandler(mockRepo.Object, mockCache.Object);

        var result = await handler.Handle(new GetAllMemberQuery(), CancellationToken.None);

        Assert.Equal(membersFromRepo.OrderBy(m => m.FirstName).ToList(), result.Data);
        mockCache.Verify(c => c.Set("members", It.IsAny<List<Member>>(), null), Times.Once);
    }
}
