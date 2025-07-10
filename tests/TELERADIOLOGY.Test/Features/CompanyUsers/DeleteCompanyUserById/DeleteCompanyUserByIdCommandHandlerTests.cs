using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.CompanyUsers.DeleteCompanyUser;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class DeleteCompanyUserByUserIdCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCompanyUserNotFound()
    {
        var mockRepo = new Mock<ICompanyUserRepository>();
        mockRepo.Setup(r => r.GetByExpressionWithTrackingAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<CompanyUser, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompanyUser?)null);

        var mockUow = new Mock<IUnitOfWork>();

        var handler = new DeleteCompanyUserByUserIdCommandHandler(mockRepo.Object, mockUow.Object);
        var userId = Guid.NewGuid();
        var command = new DeleteCompanyUserByUserIdCommand(userId);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal(new List<string> { "Şirket Kullanıcısı bulunamadı." }, result.ErrorMessages);
        mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
