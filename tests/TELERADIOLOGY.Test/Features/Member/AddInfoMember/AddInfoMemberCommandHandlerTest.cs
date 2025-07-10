using GenericRepository;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TELERADIOLOGY.Application.Features.Members;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Enums;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;
using Xunit;

public class AddInfoMemberByIdCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnFailure_When_MemberNotFound()
    {
        var memberRepoMock = new Mock<IMemberRepository>();
        var cacheServiceMock = new Mock<ICacheService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        memberRepoMock
            .Setup(x => x.GetByExpressionWithTrackingAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Member?)null);

        var handler = new AddInfoMemberByIdCommandHandler(
            memberRepoMock.Object,
            cacheServiceMock.Object,
            unitOfWorkMock.Object);

        var command = new AddInfoMemberByIdCommand(Guid.NewGuid(), "Ek bilgi açıklaması");
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal("Başvuru bulunamadı", result.ErrorMessages?.FirstOrDefault()); 
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Handle_Should_UpdateMember_When_MemberExists()
    {
        var member = new Member { Id = Guid.NewGuid() };

        var memberRepoMock = new Mock<IMemberRepository>();
        var cacheServiceMock = new Mock<ICacheService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        memberRepoMock
            .Setup(x => x.GetByExpressionWithTrackingAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(member);

        var handler = new AddInfoMemberByIdCommandHandler(
            memberRepoMock.Object,
            cacheServiceMock.Object,
            unitOfWorkMock.Object);

        var command = new AddInfoMemberByIdCommand(member.Id, "Açıklama");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccessful);
        Assert.Equal("Üyelik başvurusunda ek bilgi istendi.", result.Data); 
        Assert.Equal(ApplicationStatus.AdditionalInformationRequested, member.ApplicationStatus);
        Assert.Equal("Açıklama", member.AddInfoMessage);

        unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        cacheServiceMock.Verify(x => x.Remove("members"), Times.Once);
    }
}
