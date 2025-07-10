using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Reports.DeleteReport;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class DeleteReportByIdCommandHandlerTests
{
    private readonly Mock<IReportRepository> _reportRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteReportByIdCommandHandler _handler;

    public DeleteReportByIdCommandHandlerTests()
    {
        _reportRepositoryMock = new Mock<IReportRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteReportByIdCommandHandler(_reportRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ReportNotFound_ReturnsFailure()
    {
        _reportRepositoryMock.Setup(r => r.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Report, bool>>>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Report?)null);

        var result = await _handler.Handle(new DeleteReportByIdCommand(Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal(new List<string> { "Rapor bulunamadı." }, result.ErrorMessages); 
    }

    [Fact]
    public async Task Handle_ReportAlreadyDeleted_ReturnsFailure()
    {
        var report = new Report { Id = Guid.NewGuid(), IsDeleted = true }; 

        _reportRepositoryMock.Setup(r => r.GetByExpressionAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Report, bool>>>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(report);

        var result = await _handler.Handle(new DeleteReportByIdCommand(report.Id), CancellationToken.None); // Fix: Use 'report.Id'  

        Assert.False(result.IsSuccessful);
        Assert.Equal(new List<string> { "Rapor daha önce silinmiş." }, result.ErrorMessages);
    }

    [Fact]
    public async Task Handle_ReportDeletedSuccessfully_ReturnsSuccess()
    {
        var report = new Report { Id = Guid.NewGuid(), IsDeleted = false }; // Fix: Use Guid.NewGuid() instead of an integer for Id

        _reportRepositoryMock.Setup(r => r.GetByExpressionAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<Report, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(report);

        var result = await _handler.Handle(new DeleteReportByIdCommand(report.Id), CancellationToken.None); // Fix: Use 'report.Id' instead of hardcoded integer

        _reportRepositoryMock.Verify(r => r.Update(report), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.True(result.IsSuccessful);
        Assert.Null(result.ErrorMessages);
    }

}
