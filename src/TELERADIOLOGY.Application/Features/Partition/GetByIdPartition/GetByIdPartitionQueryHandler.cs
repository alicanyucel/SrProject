using MediatR;
using TELERADIOLOGY.Application.Extensions;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

public class GetPartitionByIdQueryHandler : IRequestHandler<GetPartitionByIdQuery, Result<Partition>>
{
    private readonly IPartitionRepository _repository;

    public GetPartitionByIdQueryHandler(IPartitionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Partition>> Handle(GetPartitionByIdQuery request, CancellationToken cancellationToken)
    {
        var partition = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (partition == null || partition.IsDeleted)
            return Result<Partition>.Failure("Partition bulunamadı.");

        return Result<Partition>.Succeed(partition);
    }
}
