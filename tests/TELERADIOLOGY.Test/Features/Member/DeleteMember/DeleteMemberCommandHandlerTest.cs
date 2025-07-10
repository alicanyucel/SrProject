using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Members.DeleteMember;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class DeleteMemberByIdCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnFailure_When_MemberNotFound()
    {
        var memberRepoMock = new Mock<IMemberRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        memberRepoMock
            .Setup(repo => repo.GetByExpressionAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Member?)null);

        var handler = new DeleteMemberByIdCommandHandler(memberRepoMock.Object, unitOfWorkMock.Object);

        var command = new DeleteMemberByIdCommand(Guid.NewGuid());
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal("Üye bulunamadı.", result.ErrorMessages?.FirstOrDefault());
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_When_MemberAlreadyDeleted()
    {
        var member = new Member { Id = Guid.NewGuid(), IsDeleted = true };

        var memberRepoMock = new Mock<IMemberRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        memberRepoMock
            .Setup(repo => repo.GetByExpressionAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(member);

        var handler = new DeleteMemberByIdCommandHandler(memberRepoMock.Object, unitOfWorkMock.Object);

        var command = new DeleteMemberByIdCommand(member.Id);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal("Üye daha önce silinmiş.", result.ErrorMessages?.FirstOrDefault()); 
    }

    [Fact]
    public async Task Handle_Should_SoftDeleteMember_When_MemberExists()
    {
        var member = new Member { Id = Guid.NewGuid(), IsDeleted = false };

        var memberRepoMock = new Mock<IMemberRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        memberRepoMock
            .Setup(repo => repo.GetByExpressionAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(member);

        var handler = new DeleteMemberByIdCommandHandler(memberRepoMock.Object, unitOfWorkMock.Object);

        var command = new DeleteMemberByIdCommand(member.Id);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccessful);
        Assert.Equal("Üye başarıyla soft silindi.", result.Data);  
        Assert.True(member.IsDeleted);

        memberRepoMock.Verify(repo => repo.Update(member), Times.Once);
        unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
