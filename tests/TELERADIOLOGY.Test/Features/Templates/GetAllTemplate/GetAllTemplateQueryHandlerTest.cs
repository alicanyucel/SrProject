using MockQueryable;
using Moq;
using TELERADIOLOGY.Application.Features.Templates.GetAllTemplate;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetAllTemplateQueryHandlerTest
{
    [Fact]
    public async Task Handle_ShouldReturnTemplatesOrderedByRaporTipi()
    {
        var templates = new List<Template>
        {
            new Template { Id = Guid.NewGuid(), RaporTipi = "Z" },
            new Template { Id = Guid.NewGuid(), RaporTipi = "A" },
            new Template { Id = Guid.NewGuid(), RaporTipi = "M" }
        };
        var mockQueryable = templates.AsQueryable().BuildMock();
        var mockTemplateRepository = new Mock<ITemplateRepository>();
        mockTemplateRepository.Setup(r => r.GetAll()).Returns(mockQueryable);
        var handler = new GetAllTemplateQueryHandler(mockTemplateRepository.Object);
        var result = await handler.Handle(new GetAllTemplateQuery(), CancellationToken.None);
        Assert.Equal(3, result.Count);
        Assert.Equal("A", result[0].RaporTipi);
        Assert.Equal("M", result[1].RaporTipi);
        Assert.Equal("Z", result[2].RaporTipi);
    }
}