using MediatR;
using TELERADIOLOGY.Application.Extensions;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

public class GetAllPartitionsQueryHandler : IRequestHandler<GetAllPartitionsQuery, Result<List<Partition>>>
{
    private readonly IPartitionRepository _repository;

    public GetAllPartitionsQueryHandler(IPartitionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<Partition>>> Handle(GetAllPartitionsQuery request, CancellationToken cancellationToken)
    {
        var partitions = await _repository.GetAllAsync(cancellationToken);
        var activePartitions = partitions
            .Where(p => !p.IsDeleted)
            .ToList();
        return Result<List<Partition>>.Succeed(activePartitions);
    }
}
