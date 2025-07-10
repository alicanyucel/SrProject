using GenericRepository;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Templates.DeleteTemplateById;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class DeleteTemplateByIdCommandHandlerTests
{
    [Fact]
    public async Task Handle_TemplateDoesNotExist_ReturnsFailure()
    {
        var templateRepositoryMock = new Mock<ITemplateRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        templateRepositoryMock
            .Setup(r => r.GetByExpressionAsync(It.IsAny<Expression<Func<Template, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Template?)null); 
        var handler = new DeleteTemplateByIdCommandHandler(
            templateRepositoryMock.Object,
            unitOfWorkMock.Object);
        var command = new DeleteTemplateByIdCommand(Guid.NewGuid()); 
        var result = await handler.Handle(command, CancellationToken.None);
        templateRepositoryMock.Verify(r => r.Delete(It.IsAny<Template>()), Times.Never);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        Assert.False(result.IsSuccessful);
        Assert.Equal(new List<string> { "Sablon bulunamadi" }, result.ErrorMessages);
    }
}
