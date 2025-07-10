using AutoMapper;
using GenericRepository;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Members.UpdateMember;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Enums;
using TELERADIOLOGY.Domain.Repositories;

public class UpdateMemberByIdCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_UpdateMember_When_MemberExists()
    { 
        var memberId = Guid.NewGuid();
        var member = new Member
        {
            Id = memberId,
            FirstName = "OldFirstName",
            LastName = "OldLastName"
        };
        var memberRepoMock = new Mock<IMemberRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();

        memberRepoMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(It.IsAny<Expression<Func<Member, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(member);
        var command = new UpdateMemberByIdCommand(
    memberId,
    "NewFirstName",
    "NewLastName",
    string.Empty,
    string.Empty,
    ApplicationStatus.Approved, 
    AreaOfInterest.ReportWrinting, 
    DateTime.UtcNow 
);
        var handler = new UpdateMemberByIdCommandHandler(memberRepoMock.Object, unitOfWorkMock.Object, mapperMock.Object); 
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.True(result.IsSuccessful);
        Assert.Equal("Üye güncellendi.", result.Data);
        mapperMock.Verify(m => m.Map(command, member), Times.Once);
        memberRepoMock.Verify(r => r.Update(member), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
