using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.DoctorSignatures.GetAllDoctorSignatureById;
using TELERADIOLOGY.Application.Features.DoctorSignatures.GetlDoctorSignatureById;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;
using Xunit;

public class GetDoctorSignatureByIdQueryHandlerTests
{
    private readonly Mock<ISignatureRepository> _repositoryMock = new();

    private GetDoctorSignatureByIdQueryHandler CreateHandler() =>
        new(_repositoryMock.Object);

    [Fact]
    public async Task Handle_SignatureNotFound_ReturnsFailure()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByExpressionAsync(x => x.Id == id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DoctorSignature?)null);

        var handler = CreateHandler();
        var query = new GetDoctorSignatureByIdQuery(id);
        var result = await handler.Handle(query, default);

        Assert.False(result.IsSuccessful);
        Assert.Equal("İmza Bulunamadı", result.ErrorMessages?.FirstOrDefault());
    }

    [Fact]
    public async Task Handle_SignatureIsDeleted_ReturnsFailure()
    {
        var id = Guid.NewGuid();
        var deletedSignature = new DoctorSignature
        {
            Id = id,
            IsDeleted = true,
            DisplayName = "Dr. Silinmiş",
            Degree = "Tıp",
            DegreeNo = "000000",
            DiplomaNo = "000000",
            RegisterNo = "TR-0000",
            Signature = new byte[] { 0x00 }
        };

        _repositoryMock
            .Setup(r => r.GetByExpressionAsync(It.IsAny<Expression<Func<DoctorSignature, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(deletedSignature);

        var handler = CreateHandler();
        var query = new GetDoctorSignatureByIdQuery(id);
        var result = await handler.Handle(query, default);

        Assert.False(result.IsSuccessful);
        Assert.Equal("İmza daha önce silinmiş.", result.ErrorMessages?.FirstOrDefault());
    }
}
