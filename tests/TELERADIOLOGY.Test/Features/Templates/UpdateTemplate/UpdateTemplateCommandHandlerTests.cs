using AutoMapper;
using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Templates.UpdateTemplate;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class UpdateTemplateByIdCommandHandlerTests
{
    private readonly Mock<ITemplateRepository> _templateRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateTemplateByIdCommandHandler _handler;

    public UpdateTemplateByIdCommandHandlerTests()
    {
        _templateRepositoryMock = new Mock<ITemplateRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _handler = new UpdateTemplateByIdCommandHandler(
            _templateRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenTemplateExists()
    {
        var command = new UpdateTemplateCommand(
            Guid.NewGuid(),
            "Updated Template",
            string.Empty,
            string.Empty,
            string.Empty // Added missing 'Content' parameter  
        );
        var template = new Template { Id = command.Id, Name = "Old Name" };

        _templateRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Template, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(template);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccessful);
        Assert.Equal("Şablon güncellendi.", result.Data);

        _mapperMock.Verify(m => m.Map(command, template), Times.Once);
        _templateRepositoryMock.Verify(r => r.Update(template), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTemplateDoesNotExist()
    {
        var command = new UpdateTemplateCommand(
            Guid.NewGuid(),
            "Nonexistent Template",
            string.Empty,
            string.Empty,
            string.Empty // Added missing 'Content' parameter  
        );

        _templateRepositoryMock
            .Setup(r => r.GetByExpressionWithTrackingAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Template, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Template)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal("Şablon yok", result.ErrorMessages?.FirstOrDefault());

        _mapperMock.Verify(m => m.Map(It.IsAny<UpdateTemplateCommand>(), It.IsAny<Template>()), Times.Never);
        _templateRepositoryMock.Verify(r => r.Update(It.IsAny<Template>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
