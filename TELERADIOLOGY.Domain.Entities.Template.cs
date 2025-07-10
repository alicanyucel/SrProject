using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TELERADIOLOGY.Application.Features.Templates.CreateTemplate;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;
using Xunit;

public class CreateTemplateCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTemplateAndReturnSuccessMessage()
    {
        var templateRepositoryMock = new Mock<ITemplateRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();

        var command = new CreateTemplateCommand
        {
            Name = "Test Template",
            Content = "Test Content"
        };

        var template = new Template { Name = "Test Template", Content = "Test Content" };

        mapperMock.Setup(m => m.Map<Template>(command)).Returns(template);

        var handler = new CreateTemplateCommandHandler(
            templateRepositoryMock.Object,
            unitOfWorkMock.Object,
            mapperMock.Object
        );

        var result = await handler.Handle(command, CancellationToken.None);

        templateRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Template>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.True(result.IsSuccess);
        Assert.Equal("Sablon kaydý yapýldý.", result.Value);
    }
}
