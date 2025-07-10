using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TELERADIOLOGY.Application.Features.Members.CreateMember;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Enums;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;
using Xunit;

public class CreateMemberCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnFailure_When_EmailAlreadyExists()
    {
        var command = new CreateMemberCommand { Email = "test@example.com" };
        var existingMember = new Member { Email = "test@example.com" };

        var memberRepoMock = new Mock<IMemberRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var cacheServiceMock = new Mock<ICacheService>();
        var mapperMock = new Mock<IMapper>();

        memberRepoMock
            .Setup(x => x.GetByExpressionAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingMember);

        var handler = new CreateMemberCommandHandler(
            memberRepoMock.Object,
            unitOfWorkMock.Object,
            cacheServiceMock.Object,
            mapperMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Bu email'e ait kullanýcý daha önce baþvurulmuþtur", result.Error);
    }

    [Fact]
    public async Task Handle_Should_CreateMember_When_EmailNotExists()
    {
        var command = new CreateMemberCommand { Email = "new@example.com" };
        var mappedMember = new Member { Email = "new@example.com" };

        var memberRepoMock = new Mock<IMemberRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var cacheServiceMock = new Mock<ICacheService>();
        var mapperMock = new Mock<IMapper>();

        memberRepoMock
            .Setup(x => x.GetByExpressionAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Member?)null);

        mapperMock.Setup(m => m.Map<Member>(command)).Returns(mappedMember);

        var handler = new CreateMemberCommandHandler(
            memberRepoMock.Object,
            unitOfWorkMock.Object,
            cacheServiceMock.Object,
            mapperMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Üye baþvurusu yapýldý", result.Value);
        Assert.Equal(ApplicationStatus.Unapproved, mappedMember.ApplicationStatus);

        memberRepoMock.Verify(r => r.Add(mappedMember), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        cacheServiceMock.Verify(c => c.Remove("members"), Times.Once);
    }
}
