using FluentAssertions;
using GenericRepository;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class DeletePartitionCommandHandlerTests
{
    private readonly Mock<IPartitionRepository> _partitionRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeletePartitionCommandHandler _handler;
    //
    public DeletePartitionCommandHandlerTests()
    {
        _partitionRepositoryMock = new Mock<IPartitionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeletePartitionCommandHandler(_partitionRepositoryMock.Object, _unitOfWorkMock.Object);
    }
    [Fact]
    public async Task Handle_Should_ReturnFailure_When_Partition_NotFound()
    {
        var partitionId = Guid.NewGuid();

        _partitionRepositoryMock
            .Setup(repo => repo.GetByExpressionAsync(
                It.IsAny<Expression<Func<Partition, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Partition?)null);

        var command = new DeletePartitionCommand(partitionId);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.IsSuccessful.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle("Silinmek istenen partition bulunamadı veya zaten silinmiş.");

        _partitionRepositoryMock.Verify(r => r.Update(It.IsAny<Partition>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
