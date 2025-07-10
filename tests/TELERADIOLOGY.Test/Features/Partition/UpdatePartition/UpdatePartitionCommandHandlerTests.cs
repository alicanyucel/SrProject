using FluentAssertions;
using GenericRepository;
using Moq;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class UpdatePartitionCommandHandlerTests
{
    private readonly Mock<IPartitionRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdatePartitionCommandHandler _handler;

    public UpdatePartitionCommandHandlerTests()
    {
        _repositoryMock = new Mock<IPartitionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdatePartitionCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Partition_When_Found_And_Not_Deleted()
    {
        var partitionId = Guid.NewGuid();
        var existingPartition = new Partition
        {
            Id = partitionId,
            IsDeleted = false,
            CompanyId = Guid.NewGuid(),
            HospitalId = Guid.NewGuid(),
            PartitionName = "Old Name",
            IsActive = true,
            Urgent = false,
            Modality = "Old Modality",
            ReferenceKey = "Old Ref",
            PartitionCode = "Old Code",
            CompanyCode = "Old Company",
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(partitionId))
            .ReturnsAsync(existingPartition);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var command = new UpdatePartitionCommand(
            CompanyId: Guid.NewGuid(),
            HospitalId: Guid.NewGuid(),
            PartitionId: partitionId,
            PartitionName: "New Name",
            IsActive: false,
            Urgent: true,
            Modality: "New Modality",
            ReferenceKey: "New Ref",
            PartitionCode: "New Code",
            CompanyCode: "New Company"
        );
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be("Partition updated successfully.");

        _repositoryMock.Verify(r => r.GetByIdAsync(partitionId), Times.Once);
        _repositoryMock.Verify(r => r.Update(It.Is<Partition>(p =>
            p.Id == partitionId &&
            p.PartitionName == "New Name" &&
            p.IsActive == false &&
            p.Urgent == true &&
            p.Modality == "New Modality" &&
            p.ReferenceKey == "New Ref" &&
            p.PartitionCode == "New Code" &&
            p.CompanyCode == "New Company"
        )), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Message_When_Partition_Not_Found()
    {
        var partitionId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(partitionId))
            .ReturnsAsync((Partition?)null);

        var command = new UpdatePartitionCommand(
            CompanyId: Guid.NewGuid(),
            HospitalId: Guid.NewGuid(),
            PartitionId: partitionId,
            PartitionName: "Name",
            IsActive: true,
            Urgent: false,
            Modality: "Modality",
            ReferenceKey: "Ref",
            PartitionCode: "Code",
            CompanyCode: "Company"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("partition yok.");
        _repositoryMock.Verify(r => r.Update(It.IsAny<Partition>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Return_Message_When_Partition_Is_Deleted()
    {
        // Arrange
        var partitionId = Guid.NewGuid();
        var deletedPartition = new Partition
        {
            Id = partitionId,
            IsDeleted = true
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(partitionId))
            .ReturnsAsync(deletedPartition);

        var command = new UpdatePartitionCommand(
            CompanyId: Guid.NewGuid(),
            HospitalId: Guid.NewGuid(),
            PartitionId: partitionId,
            PartitionName: "Name",
            IsActive: true,
            Urgent: false,
            Modality: "Modality",
            ReferenceKey: "Ref",
            PartitionCode: "Code",
            CompanyCode: "Company"
        );

        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be("partition yok.");
        _repositoryMock.Verify(r => r.Update(It.IsAny<Partition>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
