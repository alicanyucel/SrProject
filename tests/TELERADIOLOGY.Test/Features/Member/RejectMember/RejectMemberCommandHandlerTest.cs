using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Members.RejectMemberById;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Enums;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

public class RejectMemberByIdCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnFailure_When_MemberNotFound()
    {
        var memberRepoMock = new Mock<IMemberRepository>();
        var cacheServiceMock = new Mock<ICacheService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        memberRepoMock
            .Setup(repo => repo.GetByExpressionWithTrackingAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Member?)null);

        var handler = new RejectMemberByIdCommandHandler(
            memberRepoMock.Object,
            cacheServiceMock.Object,
            unitOfWorkMock.Object);

        var command = new RejectMemberByIdCommand(Guid.NewGuid());
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal(404, result.StatusCode);
        Assert.NotNull(result.ErrorMessages);   
        Assert.Contains("Başvuru bulunamadı", result.ErrorMessages!);  
    }

    [Fact]
    public async Task Handle_Should_RejectMember_When_MemberExists()
    {
        var member = new Member { Id = Guid.NewGuid() };

        var memberRepoMock = new Mock<IMemberRepository>();
        var cacheServiceMock = new Mock<ICacheService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        memberRepoMock
            .Setup(repo => repo.GetByExpressionWithTrackingAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(member);

        var handler = new RejectMemberByIdCommandHandler(
            memberRepoMock.Object,
            cacheServiceMock.Object,
            unitOfWorkMock.Object);

        var command = new RejectMemberByIdCommand(member.Id);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccessful);
        Assert.Equal("Üyelik başvurusu reddedildi", result.Data);
        Assert.Equal(ApplicationStatus.Rejected, member.ApplicationStatus);

        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        cacheServiceMock.Verify(c => c.Remove("members"), Times.Once);
    }
} 
public class RejectMemberByIdCommandHandler
{
    private readonly IMemberRepository _memberRepository;
    private readonly ICacheService _cacheService;
    private readonly IUnitOfWork _unitOfWork;

    public RejectMemberByIdCommandHandler(IMemberRepository memberRepository, ICacheService cacheService, IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _cacheService = cacheService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(RejectMemberByIdCommand command, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByExpressionWithTrackingAsync(m => m.Id == command.Id, cancellationToken);
        if (member == null)
        {
            return Result<string>.Failure(404, "Başvuru bulunamadı");
        }

        member.ApplicationStatus = ApplicationStatus.Rejected;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _cacheService.Remove("members");

        return Result<string>.Succeed("Üyelik başvurusu reddedildi");
    }
}
