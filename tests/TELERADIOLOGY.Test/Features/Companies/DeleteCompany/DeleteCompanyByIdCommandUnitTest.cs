using GenericRepository;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Companies.DeleteCompany;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Test.Features.Companies.DeleteCompany;
public sealed class DeleteCompanyByIdCommandUnitTest
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteCompanyByIdCommandHandler _handler;

    public DeleteCompanyByIdCommandUnitTest()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteCompanyByIdCommandHandler(
            _companyRepositoryMock.Object,
            _cacheServiceMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteCompany_WhenValidCompanyIdProvided()
    {
        // ARRANGE - KISS: Simple and clear
        var existingCompany = CompanyTestData.CreateCompanyForDelete();
        var command = CompanyTestData.CreateDeleteCommand(existingCompany.Id);

        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCompany);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        Assert.True(result.IsSuccessful);
        Assert.Equal("Şirket başarıyla silindi.", result.Data);
        Assert.True(existingCompany.IsDeleted);

        _cacheServiceMock.Verify(c => c.Remove("companies"), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCompanyNotFound()
    {
        // ARRANGE - KISS: Inline creation for simple case
        var command = CompanyTestData.CreateDeleteCommand();

        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company)null!);

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        Assert.False(result.IsSuccessful);
        Assert.Contains("Şirket bulunamadı.", result.ErrorMessages!);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCompanyIdIsEmpty()
    {
        // ARRANGE - KISS: Clear intent method
        var command = CompanyTestData.CreateDeleteCommandWithEmptyId();

        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        Assert.False(result.IsSuccessful);
        Assert.Contains("Şirket bulunamadı.", result.ErrorMessages!);
    }

    [Fact]
    public async Task Handle_ShouldHandleAlreadyDeletedCompany_Gracefully()
    {
        // ARRANGE - KISS: Descriptive method name tells the story
        var alreadyDeletedCompany = CompanyTestData.CreateDeletedCompany();
        var command = CompanyTestData.CreateDeleteCommand(alreadyDeletedCompany.Id);

        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(alreadyDeletedCompany);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT - Idempotent operation
        Assert.True(result.IsSuccessful);
        Assert.True(alreadyDeletedCompany.IsDeleted);
    }
}