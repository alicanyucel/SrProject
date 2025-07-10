using AutoMapper;
using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.DoctorSignatures.CreateDoctorSignature;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Test.Features.Signatures.CreateSignature;

public class CreateDoctorSignatureCommandHandlerTests
{
    private readonly Mock<ISignatureRepository> _repositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private CreateDoctorSignatureCommandHandler CreateHandler() =>
        new(
            _repositoryMock.Object,
            _mapperMock.Object,
            _unitOfWorkMock.Object
        );

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRegisterNoAlreadyExists()
    {
        // Arrange
        var command = TestData.ValidCommand;
        _repositoryMock
            .Setup(r => r.AnyAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DoctorSignature, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccessful);
        var mappedEntity = new DoctorSignature
        {
            Degree = command.Degree,
            DegreeNo = command.DegreeNo,
            DiplomaNo = command.DiplomaNo,
            RegisterNo = command.RegisterNo,
            Signature = command.Signature,
            DisplayName = command.DisplayName
        }; // Kendi entity'ni oluştur
        Assert.Equal(new List<string> { "Bu sicil numarası ile daha önce kayıt yapılmış." }, result.ErrorMessages!);
        _repositoryMock.Verify(r => r.Add(It.IsAny<DoctorSignature>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldAddAndSave_WhenRegisterNoIsUnique()
    {
        // Arrange  
        var command = TestData.ValidCommand;
        _repositoryMock
            .Setup(r => r.AnyAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DoctorSignature, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var mappedEntity = new DoctorSignature
        {
            Degree = command.Degree,
            DegreeNo = command.DegreeNo,
            DiplomaNo = command.DiplomaNo,
            RegisterNo = command.RegisterNo,
            Signature = command.Signature,
            DisplayName = command.DisplayName
        };

        _mapperMock
            .Setup(m => m.Map<DoctorSignature>(command))
            .Returns(mappedEntity);

        var handler = CreateHandler();

        // Act  
        var result = await handler.Handle(command, default);

        // Assert  
        Assert.True(result.IsSuccessful);
        Assert.Equal("Doktor imzası başarıyla kaydedildi.", result.Data);
        _repositoryMock.Verify(r => r.Add(mappedEntity), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

public static class TestData
{
    public static CreateDoctorSignatureCommand ValidCommand => new(
        Degree: "Uzm. Dr.",
        DegreeNo: "12345",
        DiplomaNo: "67890",
        RegisterNo: "REG-001",
        DisplayName: "Dr. Emre Can",
        Signature: new byte[] { 1, 2, 3 }
    );
}
