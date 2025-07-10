using AutoMapper;
using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Members.CreateMember;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Enums;
using TELERADIOLOGY.Domain.Repositories;
    
public class CreateMemberCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnFailure_When_EmailAlreadyExists()
    {
        var command = new CreateMemberCommand(
            FirstName: "Test",
            LastName: "User",
            IdentityNumber: "12345678901",
            Phone: "5555555555",
            Email: "test@example.com",
            AreaOfInterest: AreaOfInterest.ReportReading
        );
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

        Assert.False(result.IsSuccessful);
        Assert.Equal("Bu email'e ait kullanıcı daha önce başvurulmuştur", result.ErrorMessages?.FirstOrDefault());
    }

    [Fact]
    public async Task Handle_Should_CreateMember_When_EmailNotExists()
    {
        var command = new CreateMemberCommand(
            FirstName: "New",
            LastName: "User",
            IdentityNumber: "98765432101",
            Phone: "4444444444",
            Email: "new@example.com",
            AreaOfInterest: AreaOfInterest.ReportWrinting
        );
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

        Assert.True(result.IsSuccessful);
        Assert.Equal("Üye başvurusu yapıldı", result.Data); 
        Assert.Equal(ApplicationStatus.Unapproved, mappedMember.ApplicationStatus);

        memberRepoMock.Verify(r => r.Add(mappedMember), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        cacheServiceMock.Verify(c => c.Remove("members"), Times.Once);
    }
}
