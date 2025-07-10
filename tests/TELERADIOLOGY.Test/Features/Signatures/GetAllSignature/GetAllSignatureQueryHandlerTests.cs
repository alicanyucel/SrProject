using MockQueryable;
using Moq;
using TELERADIOLOGY.Application.Features.DoctorSignatures.GetAllDoctorSignature;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Test.Features.Signatures.GetAllSignature;

public class GetAllDoctorSignatureQueryHandlerTests
{
    private readonly Mock<ISignatureRepository> _repositoryMock = new();

    private GetAllDoctorSignatureQueryHandler CreateHandler() =>
        new(_repositoryMock.Object);

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSignaturesExist()
    {
        var emptyList = new List<DoctorSignature>();
        var mockQueryable = emptyList.AsQueryable().BuildMock();

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        var handler = CreateHandler();
        var query = new GetAllDoctorSignatureQuery();
        var result = await handler.Handle(query, default);

        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllSignatures_WhenSignaturesExist()
    {
        var signatures = TestData.CreateDoctorSignatureList();
        var mockQueryable = signatures.AsQueryable().BuildMock();

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        var handler = CreateHandler();
        var query = new GetAllDoctorSignatureQuery();
        var result = await handler.Handle(query, default);

        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Data);
        Assert.Equal(signatures.Count, result.Data.Count);
    }

    [Fact]
    public async Task Handle_ShouldReturnSignaturesOrderedByDisplayName()
    {
        var signatures = TestData.CreateUnorderedDoctorSignatureList();
        var mockQueryable = signatures.AsQueryable().BuildMock();

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        var handler = CreateHandler();
        var query = new GetAllDoctorSignatureQuery();
        var result = await handler.Handle(query, default);

        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Data);
        var expectedOrder = signatures.OrderBy(s => s.DisplayName).ToList();
        for (int i = 0; i < result.Data.Count; i++)
        {
            Assert.Equal(expectedOrder[i].DisplayName, result.Data[i].DisplayName);
        }
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryGetAllOnce()
    {
        var signatures = TestData.CreateDoctorSignatureList();
        var mockQueryable = signatures.AsQueryable().BuildMock();

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        var handler = CreateHandler();
        var query = new GetAllDoctorSignatureQuery();

        await handler.Handle(query, default);

        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_Always()
    {
        var signatures = TestData.CreateDoctorSignatureList();
        var mockQueryable = signatures.AsQueryable().BuildMock();

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns(mockQueryable);

        var handler = CreateHandler();
        var query = new GetAllDoctorSignatureQuery();
        var result = await handler.Handle(query, default);

        Assert.True(result.IsSuccessful);
        Assert.Null(result.ErrorMessages);
    }
}

public static class TestData
{
    public static List<DoctorSignature> CreateDoctorSignatureList() => new()
    {
        new DoctorSignature
        {
            Id = Guid.NewGuid(),
            Degree = "Uzm. Dr.",
            DegreeNo = "12345",
            DiplomaNo = "67890",
            RegisterNo = "REG-001",
            DisplayName = "Dr. Ahmet Yılmaz",
            Signature = new byte[] { 1, 2, 3 },
            IsDeleted = false
        },
        new DoctorSignature
        {
            Id = Guid.NewGuid(),
            Degree = "Prof. Dr.",
            DegreeNo = "54321",
            DiplomaNo = "09876",
            RegisterNo = "REG-002",
            DisplayName = "Dr. Emre Can",
            Signature = new byte[] { 4, 5, 6 },
            IsDeleted = false
        }
    };

    public static List<DoctorSignature> CreateUnorderedDoctorSignatureList() => new()
    {
        new DoctorSignature
        {
            Id = Guid.NewGuid(),
            Degree = "Uzm. Dr.",
            DegreeNo = "12345",
            DiplomaNo = "67890",
            RegisterNo = "REG-003",
            DisplayName = "Dr. Zeynep Kaya",
            Signature = new byte[] { 7, 8, 9 },
            IsDeleted = false
        },
        new DoctorSignature
        {
            Id = Guid.NewGuid(),
            Degree = "Prof. Dr.",
            DegreeNo = "54321",
            DiplomaNo = "09876",
            RegisterNo = "REG-004",
            DisplayName = "Dr. Ahmet Demir",
            Signature = new byte[] { 10, 11, 12 },
            IsDeleted = false
        },
        new DoctorSignature
        {
            Id = Guid.NewGuid(),
            Degree = "Doç. Dr.",
            DegreeNo = "11111",
            DiplomaNo = "22222",
            RegisterNo = "REG-005",
            DisplayName = "Dr. Mehmet Özkan",
            Signature = new byte[] { 13, 14, 15 },
            IsDeleted = false
        }
    };
}
