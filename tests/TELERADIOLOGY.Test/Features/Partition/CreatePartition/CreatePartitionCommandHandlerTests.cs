using AutoMapper;
using GenericRepository;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Partitions.CreatePartition;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class CreatePartitionCommandHandlerTests
{
    private readonly Mock<IPartitionRepository> _partitionRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreatePartitionCommandHandler _handler;

    public CreatePartitionCommandHandlerTests()
    {
        _partitionRepositoryMock = new Mock<IPartitionRepository>();
        _mapperMock = new Mock<IMapper>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreatePartitionCommandHandler(
            _partitionRepositoryMock.Object,
            _mapperMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPartitionCodeAlreadyExists()
    {
        var command = new CreatePartitionCommand
        (
            CompanyId: Guid.NewGuid(),
            HospitalId: Guid.NewGuid(),
            PartitionName: "TestPartition",
            IsActive: true,
            Urgent: false,
            Modality: "CT",
            ReferenceKey: "Ref123",
            PartitionCode: "P123",
            CompanyCode: "C123"
        );

        _partitionRepositoryMock
            .Setup(repo => repo.AnyAsync(
                It.IsAny<Expression<Func<Partition, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccessful);
        Assert.Equal("Bu Partition kodu ile zaten bir kayıt mevcut.", result.ErrorMessages?.FirstOrDefault());
    }

    [Fact]
    public async Task Handle_ShouldCreatePartition_WhenPartitionCodeDoesNotExist()
    {
        var command = new CreatePartitionCommand
        (
            CompanyId: Guid.NewGuid(),
            HospitalId: Guid.NewGuid(),
            PartitionName: "TestPartition",
            IsActive: true,
            Urgent: false,
            Modality: "CT",
            ReferenceKey: "Ref123",
            PartitionCode: "P123",
            CompanyCode: "C123"
        );

        _partitionRepositoryMock
            .Setup(repo => repo.AnyAsync(
                It.IsAny<Expression<Func<Partition, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var partition = new Partition();
        _mapperMock
            .Setup(m => m.Map<Partition>(command))
            .Returns(partition);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccessful);
        Assert.Equal("Partition başarıyla oluşturuldu.", result.Data);
        _partitionRepositoryMock.Verify(repo => repo.Add(partition), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
