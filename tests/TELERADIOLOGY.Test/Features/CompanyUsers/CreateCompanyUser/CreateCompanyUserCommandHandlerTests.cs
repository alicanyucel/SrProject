using GenericRepository;
using MockQueryable;
using Moq;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class CreateCompanyUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCompanyNotFound()
    {
        var mockCompanyRepo = new Mock<ICompanyRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockCompanyUserRepo = new Mock<ICompanyUserRepository>();
        var mockUow = new Mock<IUnitOfWork>();

        mockCompanyRepo.Setup(r => r
            .WhereWithTracking(It.IsAny<System.Linq.Expressions.Expression<Func<Company, bool>>>()))
            .Returns(new List<Company>().AsQueryable().BuildMock());

        var handler = new CreateCompanyUserCommandHandler(
            mockCompanyRepo.Object,
            mockUserRepo.Object,
            mockCompanyUserRepo.Object,
            mockUow.Object
        );

        var command = new CreateCompanyUserCommand(
            Guid.NewGuid(),
            "12345678901",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            false,
            DateTime.MinValue,
            null
        );

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal(new List<string> { "Şirket bulunamadı." }, result.ErrorMessages);
    }
}
