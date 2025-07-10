using AutoMapper;
using FluentAssertions;
using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Hospitals.UpdateHospital;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class UpdateHospitalCommandHandlerTests
{
    private readonly Mock<IHospitalRepository> _hospitalRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateHospitalCommandHandler _handler;

    public UpdateHospitalCommandHandlerTests()
    {
        _hospitalRepositoryMock = new Mock<IHospitalRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateHospitalCommandHandler(
            _hospitalRepositoryMock.Object,
            _mapperMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHospitalNotFound()
    {
        var command = new UpdateHospitalCommand
        (
            Id: Guid.NewGuid(),
            ShortName: string.Empty,
            FullTitle: string.Empty,
            AuthorizedPerson: string.Empty,
            City: string.Empty,
            District: string.Empty,
            Phone: string.Empty,
            Email: string.Empty,
            Address: string.Empty,
            TaxNumber: string.Empty,
            TaxOffice: string.Empty,
            Website: string.Empty,
            IsActive: false
        );

        _hospitalRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Hospital, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Hospital)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Bu id'ye uygun kayıt bulunamadı");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTaxNumberAlreadyExists()
    {
        var command = new UpdateHospitalCommand
        (
            Id: Guid.NewGuid(),
            ShortName: string.Empty,
            FullTitle: string.Empty,
            AuthorizedPerson: string.Empty,
            City: string.Empty,
            District: string.Empty,
            Phone: string.Empty,
            Email: string.Empty,
            Address: string.Empty,
            TaxNumber: "999",
            TaxOffice: string.Empty,
            Website: string.Empty,
            IsActive: false
        );

        var existingHospital = new Hospital { Id = Guid.NewGuid(), TaxNumber = "123" };

        _hospitalRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Hospital, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHospital);

        _hospitalRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Hospital, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Bu vergi numarası ile daha önce kayıt oluşturulmuş");
    }

    [Fact]
    public async Task Handle_ShouldUpdateHospital_WhenValidRequest()
    {
        var command = new UpdateHospitalCommand
        (
            Id: Guid.NewGuid(),
            ShortName: "ShortName",
            FullTitle: "FullTitle",
            AuthorizedPerson: "AuthorizedPerson",
            City: "City",
            District: "District",
            Phone: "Phone",
            Email: "Email",
            Address: "Address",
            TaxNumber: "456",
            TaxOffice: "TaxOffice",
            Website: "Website",
            IsActive: true
        );

        var existingHospital = new Hospital { Id = Guid.NewGuid(), TaxNumber = "123" };

        _hospitalRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Hospital, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHospital);

        _hospitalRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Hospital, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().Be("Hastane kaydı başarıyla güncellendi");

        _mapperMock.Verify(m => m.Map(command, existingHospital), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
