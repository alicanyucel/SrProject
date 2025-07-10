using AutoMapper;
using FluentAssertions;
using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Hospitals.CreateHospital;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class CreateHospitalCommandHandlerTests
{
    private readonly Mock<IHospitalRepository> _hospitalRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateHospitalCommandHandler _handler;

    public CreateHospitalCommandHandlerTests()
    {
        _hospitalRepositoryMock = new Mock<IHospitalRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateHospitalCommandHandler(
            _hospitalRepositoryMock.Object,
            _mapperMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTaxNumberAlreadyExists()
    {
        var command = new CreateHospitalCommand(
            ShortName: "Test Hospital",
            FullTitle: string.Empty,
            AuthorizedPerson: string.Empty,
            City: string.Empty,
            District: string.Empty,
            Phone: string.Empty,
            Email: string.Empty,
            Address: string.Empty,
            TaxNumber: "1234567890",
            TaxOffice: string.Empty,
            Website: string.Empty,
            IsActive: true
        );

        _hospitalRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Hospital, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

       
        var result = await _handler.Handle(command, CancellationToken.None);

      
        result.IsSuccessful.Should().BeFalse(); // Replace IsFailure with IsSuccessful and check for false  
        result.IsSuccessful.Should().BeFalse("Bu vergi numarası ile daha önce kayıt oluşturulmuş.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenHospitalIsCreated()
    {
       
        var command = new CreateHospitalCommand(
            ShortName: "New Hospital",
            FullTitle: string.Empty, 
            AuthorizedPerson: string.Empty,
            City: string.Empty, 
            District: string.Empty,
            Phone: string.Empty,   
            Email: string.Empty,  
            Address: string.Empty,
            TaxNumber: "9876543210",
            TaxOffice: string.Empty,   
            Website: string.Empty,   
            IsActive: true
        );

        var hospital = new Hospital { ShortName = command.ShortName, TaxNumber = command.TaxNumber };

        _hospitalRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Hospital, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mapperMock
            .Setup(m => m.Map<Hospital>(command))
            .Returns(hospital);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

       
        var result = await _handler.Handle(command, CancellationToken.None);

     
        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().Be("Hastane başarıyla kaydedildi.");

        _hospitalRepositoryMock.Verify(r => r.Add(It.Is<Hospital>(h => h.ShortName == hospital.ShortName && h.TaxNumber == hospital.TaxNumber)), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
