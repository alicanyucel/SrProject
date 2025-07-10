using MockQueryable;
using Moq;
using TELERADIOLOGY.Application.Features.CompanyUsers.GetAllCompanyUser;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetAllCompanyUsersQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCompanyUserList_WhenCompanyUsersExist()
    {
        var fakeData = new List<CompanyUser>
        {
            new CompanyUser
            {
                UserId = Guid.NewGuid(),
                IsDeleted = false,
                IsActive = true,
                StartDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now.AddMonths(1),
                CompanyId = Guid.NewGuid(),
                User = new AppUser
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "1234567890"
                },
                Company = new TELERADIOLOGY.Domain.Entities.Company
                {
                    CompanyTitle = "Test Company"
                }
            }
        }.AsQueryable();

        var mockRepo = new Mock<ICompanyUserRepository>();

        // Correct usage of BuildMock
        mockRepo.Setup(r => r.Where(It.IsAny<System.Linq.Expressions.Expression<Func<CompanyUser, bool>>>()))
                .Returns(fakeData.BuildMock()); 
        var handler = new GetAllCompanyUsersQueryHandler(mockRepo.Object);
        var result = await handler.Handle(new GetAllCompanyUsersQuery(), CancellationToken.None);
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("John", result[0].FirstName);
        Assert.Equal("Doe", result[0].LastName);
        Assert.Equal("Test Company", result[0].CompanyTitle);
    }
}

