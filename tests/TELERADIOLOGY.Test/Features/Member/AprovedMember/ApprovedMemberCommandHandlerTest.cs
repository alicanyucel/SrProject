using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Members.ApproveMemberById;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class ApproveMemberByIdCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnFailure_When_MemberNotFound()
    {
        var memberRepoMock = new Mock<IMemberRepository>();
        var loginRepoMock = new Mock<ILoginRepository>();
        var userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(),
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);
        var cacheServiceMock = new Mock<ICacheService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var passwordHasherMock = new Mock<IPasswordHasher<Login>>();

        // Fix for CS1503: Use Expression<Func<Member, bool>> instead of Func<Member, bool>  
        memberRepoMock.Setup(x => x.GetByExpressionWithTrackingAsync(It.IsAny<Expression<Func<Member, bool>>>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync((Member?)null);

        var handler = new ApproveMemberByIdCommandHandler(
            memberRepoMock.Object,
            loginRepoMock.Object,
            userManagerMock.Object,
            cacheServiceMock.Object,
            unitOfWorkMock.Object,
            passwordHasherMock.Object
        );

        var command = new ApproveMemberByIdCommand(
            Guid.NewGuid(),
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            "123",
            "123",
            string.Empty,
            Guid.Empty
        );
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal(500, result.StatusCode);
        Assert.NotNull(result.ErrorMessages);
        Assert.Contains("Başvuru bulunamadı", result.ErrorMessages);
    }

}
