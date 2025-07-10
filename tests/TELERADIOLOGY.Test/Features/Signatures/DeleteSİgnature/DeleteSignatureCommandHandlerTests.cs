using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.DoctorSignatures.DeleteDoctorSignature;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Test.Features.Signatures.DeleteSİgnature;


public class DeleteDoctorSignatureByIdCommandHandlerTests
{
    private readonly Mock<ISignatureRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private DeleteDoctorSignatureByIdCommandHandler CreateHandler() =>
        new(
            _repositoryMock.Object,
            _unitOfWorkMock.Object
        );

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSignatureNotFound()
    {
        var command = TestData.ValidDeleteCommand;
        _repositoryMock
            .Setup(r => r.GetByExpressionAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DoctorSignature, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((DoctorSignature)null!);
        var handler = CreateHandler();
        var result = await handler.Handle(command, default);
        Assert.False(result.IsSuccessful);
        Assert.Equal(new List<string> { "İmza bulunamadı." }, result.ErrorMessages);
        _repositoryMock.Verify(r => r.Update(It.IsAny<DoctorSignature>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSignatureAlreadyDeleted()
    {
        var command = TestData.ValidDeleteCommand;
        var deletedSignature = TestData.CreateDoctorSignature(isDeleted: true);

        _repositoryMock
            .Setup(r => r.GetByExpressionAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DoctorSignature, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(deletedSignature);

        var handler = CreateHandler();  
        var result = await handler.Handle(command, default);
        Assert.False(result.IsSuccessful);
        Assert.NotNull(result.ErrorMessages); 
        Assert.Contains("Signature daha önce silinmiş.", result.ErrorMessages!);
        _repositoryMock.Verify(r => r.Update(It.IsAny<DoctorSignature>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldSoftDeleteSignature_WhenSignatureExistsAndNotDeleted()
    { 
        var command = TestData.ValidDeleteCommand;
        var activeSignature = TestData.CreateDoctorSignature(isDeleted: false);
        _repositoryMock
            .Setup(r => r.GetByExpressionAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DoctorSignature, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(activeSignature);
        var handler = CreateHandler();
        var result = await handler.Handle(command, default);
        Assert.True(result.IsSuccessful);
        Assert.Equal("Doktor imzası başarıyla soft silindi.", result.Data);
        Assert.True(activeSignature.IsDeleted); 
        _repositoryMock.Verify(r => r.Update(activeSignature), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        var signatureId = Guid.NewGuid();
        var command = new DeleteDoctorSignatureByIdCommand(signatureId);
        var activeSignature = TestData.CreateDoctorSignature(isDeleted: false);
        _repositoryMock
            .Setup(r => r.GetByExpressionAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DoctorSignature, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(activeSignature);

        var handler = CreateHandler();
        await handler.Handle(command, default);
        _repositoryMock.Verify(
            r => r.GetByExpressionAsync(
                It.Is<System.Linq.Expressions.Expression<Func<DoctorSignature, bool>>>(
                    expr => expr.Compile()(new DoctorSignature
                    {
                        Id = signatureId,
                        Signature = new byte[] { 0 },
                        RegisterNo = "REG-001",
                        DiplomaNo = "67890",
                        DegreeNo = "12345", 
                        Degree = "Uzm. Dr." 
                    })),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

public static class TestData
{
    public static DeleteDoctorSignatureByIdCommand ValidDeleteCommand =>
        new(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));

    public static DoctorSignature CreateDoctorSignature(bool isDeleted = false) =>
        new()
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
            Degree = "Uzm. Dr.",
            DegreeNo = "12345",
            DiplomaNo = "67890",
            RegisterNo = "REG-001",
            DisplayName = "Dr. Emre Can",
            Signature = new byte[] { 1, 2, 3 },
            IsDeleted = isDeleted
        };
}
