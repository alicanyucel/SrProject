using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Partitions.CreatePartition;

internal sealed class CreatePartitionCommandHandler(
    IPartitionRepository partitionRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreatePartitionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreatePartitionCommand request, CancellationToken cancellationToken)
    {
        // PartitionCode + CompanyId eşsiz mi kontrolü
        bool exists = await partitionRepository.AnyAsync(
            p => p.PartitionCode == request.PartitionCode &&
                 p.CompanyId == request.CompanyId &&
                 !p.IsDeleted,
            cancellationToken
        );

        if (exists)
            return Result<string>.Failure("Bu Partition kodu ile zaten bir kayıt mevcut.");

        Partition partition = mapper.Map<Partition>(request);

        partition.Id = Guid.NewGuid(); // Base Entity'deki Id
        partition.CreatedAt = DateTime.UtcNow;
        partition.UpdatedAt = DateTime.UtcNow;
        partition.IsDeleted = false;

        partitionRepository.Add(partition);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Partition başarıyla oluşturuldu.");
    }
}
