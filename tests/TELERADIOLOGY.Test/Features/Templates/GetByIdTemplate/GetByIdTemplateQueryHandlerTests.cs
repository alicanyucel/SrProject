using MockQueryable;
using Moq;
using TELERADIOLOGY.Application.Features.Templates.GetTemplateById;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetTemplateByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCorrectTemplate_WhenIdMatches()
    {
        var id = Guid.NewGuid();
        var templates = new List<Template>
        {
            new Template { Id = id, RaporTipi = "A" },
            new Template { Id = Guid.NewGuid(), RaporTipi = "B" }
        };
        var mockQueryable = templates.AsQueryable().BuildMock();
        var mockRepo = new Mock<ITemplateRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(mockQueryable);
        var handler = new GetTemplateByIdQueryHandler(mockRepo.Object);
        var query = new GetTemplateByIdQuery(id);
        var result = await handler.Handle(query, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
}