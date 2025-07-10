using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Companies.CreateCompany;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Test.Features.Companies.CreateCompany;
public sealed class CreateCompanyCommandUnitTest
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateCompanyCommandHandler _handler;

    public CreateCompanyCommandUnitTest()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateCompanyCommandHandler(
            _companyRepositoryMock.Object,
            _cacheServiceMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenValidCommandProvided()
    {
        // ARRANGE 
        var command = CompanyTestData.CreateValidCommand();

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        Assert.True(result.IsSuccessful);
        Assert.Equal("Şirket kaydı yapıldı.", result.Data);

        
        _companyRepositoryMock.Verify(r =>
            r.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _cacheServiceMock.Verify(c =>
            c.Remove("companies"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenInvalidCompanyTypeProvided()
    {
        // ARRANGE - KISS: One line, obvious intent
        var command = CompanyTestData.CreateInvalidCommand();

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        Assert.False(result.IsSuccessful);
        Assert.Contains("Geçersiz CompanyType değeri.", result.ErrorMessages!);

        // Verify no side effects
        _companyRepositoryMock.Verify(r =>
            r.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmptyTitleProvided()
    {
        // ARRANGE - KISS: Simple modification
        var command = CompanyTestData.CreateValidCommand()
            .WithTitle("");

        // ACT & ASSERT - Would depend on your validation logic
        var result = await _handler.Handle(command, CancellationToken.None);

        // Add assertions based on your business rules...
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSaveChangesThrowsException()
    {
        // ARRANGE
        var command = CompanyTestData.CreateValidCommand();

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        Assert.False(result.IsSuccessful);
        Assert.Contains("Database error", result.ErrorMessages!);
    }
}