using GenericRepository;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Companies.UpdateCompany;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Test.Features.Companies.UpdateCompany;

public sealed class UpdateCompanyCommandUnitTest
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateCompanyCommandHandler _handler;

    public UpdateCompanyCommandUnitTest()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateCompanyCommandHandler(
            _companyRepositoryMock.Object,
            _cacheServiceMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenValidCommandProvided()
    {
        var companyId = Guid.NewGuid();
        var existingCompany = CompanyTestData.CreateCompanyForUpdate(companyId);
        var command = CompanyTestData.CreateValidUpdateCommand(companyId);
        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCompany);
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result.IsSuccessful);
        Assert.Equal("Şirket bilgileri güncellendi.", result.Data);
        _companyRepositoryMock.Verify(r =>
            r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        _cacheServiceMock.Verify(c =>
            c.Remove("companies"),
            Times.Once);
        Assert.Equal(command.CompanyTitle, existingCompany.CompanyTitle);
        Assert.Equal(command.CompanySmallTitle, existingCompany.CompanySmallTitle);
        Assert.Equal(command.Email, existingCompany.Email);
        Assert.Equal(command.Status, existingCompany.Status);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCompanyNotFound()
    {
        var command = CompanyTestData.CreateUpdateCommandForNonExistentCompany();
        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Company?)null);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Şirket bulunamadı.", result.ErrorMessages!);
        _companyRepositoryMock.Verify(r =>
            r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
        _cacheServiceMock.Verify(c =>
            c.Remove("companies"),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCompanyIsDeleted()
    {
        var companyId = Guid.NewGuid();
        var deletedCompany = CompanyTestData.CreateDeletedCompanyForUpdate(companyId);
        var command = CompanyTestData.CreateValidUpdateCommand(companyId);
        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(deletedCompany);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Silinmiş şirket güncellenemez.", result.ErrorMessages!);
        _companyRepositoryMock.Verify(r =>
            r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
        _cacheServiceMock.Verify(c =>
            c.Remove("companies"),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenInvalidCompanyTypeProvided()
    {
        var companyId = Guid.NewGuid();
        var existingCompany = CompanyTestData.CreateCompanyForUpdate(companyId);
        var command = CompanyTestData.CreateInvalidUpdateCommand(companyId);
        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCompany);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Geçersiz CompanyType değeri.", result.ErrorMessages!);
        _companyRepositoryMock.Verify(r =>
            r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
        _cacheServiceMock.Verify(c =>
            c.Remove("companies"),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSaveChangesThrowsException()
    {
        var companyId = Guid.NewGuid();
        var existingCompany = CompanyTestData.CreateCompanyForUpdate(companyId);
        var command = CompanyTestData.CreateValidUpdateCommand(companyId);
        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCompany);
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Database error", result.ErrorMessages!);
        _companyRepositoryMock.Verify(r =>
            r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        _cacheServiceMock.Verify(c =>
            c.Remove("companies"),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSaveChangesThrowsExceptionWithInnerException()
    {
        var companyId = Guid.NewGuid();
        var existingCompany = CompanyTestData.CreateCompanyForUpdate(companyId);
        var command = CompanyTestData.CreateValidUpdateCommand(companyId);
        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCompany);
        var innerException = new Exception("Inner database error");
        var outerException = new InvalidOperationException("Outer error", innerException);
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(outerException);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Inner database error", result.ErrorMessages!);
        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_ShouldUpdateStatus_WhenDifferentStatusValues(bool newStatus)
    {
        var companyId = Guid.NewGuid();
        var existingCompany = CompanyTestData.CreateCompanyForUpdate(companyId);
        existingCompany.Status = !newStatus;
        var command = CompanyTestData.CreateValidUpdateCommand(companyId).WithStatus(newStatus);
        _companyRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCompany);
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result.IsSuccessful);
        Assert.Equal(newStatus, existingCompany.Status);
    }
}
