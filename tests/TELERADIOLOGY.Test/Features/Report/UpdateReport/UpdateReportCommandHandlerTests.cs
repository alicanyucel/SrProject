using AutoMapper;
using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Reports.UpdateReport;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class UpdateReportByIdCommandHandlerTests
{
    [Fact]
    public async Task Handle_ReportExists_UpdatesReportAndReturnsSuccess()
    {
        var reportId = Guid.NewGuid();
        var command = new UpdateReportCommand(
            reportId,
            "Yeni Rapor",
            true,
            "CT"
        );

        var existingReport = new Report { Id = reportId, ReportName = "Eski Rapor" };

        var repoMock = new Mock<IReportRepository>();
        repoMock.Setup(r => r.GetByExpressionWithTrackingAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Report, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingReport);
        repoMock.Setup(r => r.Update(It.IsAny<Report>()));

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map(It.IsAny<UpdateReportCommand>(), It.IsAny<Report>()));

        var handler = new UpdateReportByIdCommandHandler(repoMock.Object, unitOfWorkMock.Object, mapperMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        repoMock.Verify(r => r.GetByExpressionWithTrackingAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Report, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        mapperMock.Verify(m => m.Map(command, existingReport), Times.Once);
        repoMock.Verify(r => r.Update(existingReport), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.True(result.IsSuccessful);
        Assert.Equal("Rapor güncellendi.", result.Data);
    }

    [Fact]
    public async Task Handle_ReportDoesNotExist_ReturnsFailure()
    {
        var command = new UpdateReportCommand(
            Guid.NewGuid(),
            "Yeni Rapor",
            true,
            "CT"
        );

        var repoMock = new Mock<IReportRepository>();
        repoMock.Setup(r => r.GetByExpressionWithTrackingAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Report, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Report?)null);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();

        var handler = new UpdateReportByIdCommandHandler(repoMock.Object, unitOfWorkMock.Object, mapperMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.NotNull(result.ErrorMessages);
        Assert.Contains("rapor yok", result.ErrorMessages);
    }
}
