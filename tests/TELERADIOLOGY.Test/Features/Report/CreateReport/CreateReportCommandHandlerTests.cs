namespace TELERADIOLOGY.Test.Features.Report.CreateReport
{
    using AutoMapper;
    using GenericRepository;
    using Moq;
    using System.Threading;
    using System.Threading.Tasks;
    using TELERADIOLOGY.Application.Features.Reports.CreateReport;
    using TELERADIOLOGY.Domain.Entities;
    using TELERADIOLOGY.Domain.Repositories;
    using Xunit;

    public class CreateReportCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Create_Report_And_Return_Success_Message()
        {
            var reportRepositoryMock = new Mock<IReportRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var command = new CreateReportCommand(
                ReportName: "Test Report",
                Emergency: false,
                ModalityType: "CT",
                TemplateId: Guid.NewGuid()
            );
            var report = new Report();

            mapperMock.Setup(m => m.Map<Report>(command)).Returns(report);
            reportRepositoryMock.Setup(r => r.AddAsync(report, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1); 
            var handler = new CreateReportCommandHandler(reportRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.True(result.IsSuccessful);
            Assert.Equal("Rapor kaydı yapıldı.", result.Data);
            reportRepositoryMock.Verify(r => r.AddAsync(report, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
