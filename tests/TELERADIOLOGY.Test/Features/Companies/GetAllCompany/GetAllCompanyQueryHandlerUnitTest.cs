using Microsoft.EntityFrameworkCore;
using Moq;
using TELERADIOLOGY.Application.Features.Companies.GetAllCompany;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetAllCompanyQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenCompaniesInCache_ReturnsCachedCompanies()
    {
        // Arrange
        var cachedCompanies = new List<Company>
        {
            new Company { CompanyTitle = "Cached Company 1" },
            new Company { CompanyTitle = "Cached Company 2" }
        };

        var mockRepo = new Mock<ICompanyRepository>();
        var mockCache = new Mock<ICacheService>();

        mockCache.Setup(c => c.Get<List<Company>>("companies")).Returns(cachedCompanies);

        var handler = new GetAllCompanyQueryHandler(mockRepo.Object, mockCache.Object);

        // Act
        var result = await handler.Handle(new GetAllCompanyQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result?.Data?.Count);
        mockRepo.Verify(r => r.GetAll(), Times.Never);  // Repo should not be called since cache hit
    }

    

    // Helper method to mock DbSet<T>
    private static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
    {
        var queryable = sourceList.AsQueryable();
        var dbSet = new Mock<DbSet<T>>();
        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        return dbSet;
    }
}
