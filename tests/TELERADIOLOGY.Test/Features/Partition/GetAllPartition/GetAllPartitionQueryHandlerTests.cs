using FluentAssertions;
using Moq;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetAllPartitionsQueryHandlerTests
{
    private readonly Mock<IPartitionRepository> _repositoryMock;
    private readonly GetAllPartitionsQueryHandler _handler;

    public GetAllPartitionsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IPartitionRepository>();
        _handler = new GetAllPartitionsQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Only_Active_Partitions()
    {
        // Arrange
        var partitions = new List<Partition>
        {
            new Partition { Id = Guid.NewGuid(), PartitionName = "Partition 1", IsDeleted = false },
            new Partition { Id = Guid.NewGuid(), PartitionName = "Partition 2", IsDeleted = true },
            new Partition { Id = Guid.NewGuid(), PartitionName = "Partition 3", IsDeleted = false }
        }.AsQueryable();

        _repositoryMock.Setup(r => r.GetAll())
            .Returns(new TestAsyncEnumerable<Partition>(partitions));

        var query = new GetAllPartitionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().OnlyContain(p => !p.IsDeleted);

        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_EmptyList_When_No_Active_Partitions()
    {
        // Arrange
        var partitions = new List<Partition>
        {
            new Partition { Id = Guid.NewGuid(), IsDeleted = true },
            new Partition { Id = Guid.NewGuid(), IsDeleted = true }
        }.AsQueryable();

        _repositoryMock.Setup(r => r.GetAll())
            .Returns(new TestAsyncEnumerable<Partition>(partitions));

        var query = new GetAllPartitionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().BeEmpty();

        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }
}
