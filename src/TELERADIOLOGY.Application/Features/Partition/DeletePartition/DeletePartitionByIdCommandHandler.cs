using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Extensions;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

public class DeletePartitionCommandHandler : IRequestHandler<DeletePartitionCommand, Result<string>>
{
    private readonly IPartitionRepository _repository;
    private readonly IUnitOfWork _unitofwork;

    public DeletePartitionCommandHandler(IPartitionRepository repository, IUnitOfWork unitofwork)
    {
        _repository = repository;
        _unitofwork = unitofwork;
    }

    public async Task<Result<string>> Handle(DeletePartitionCommand request, CancellationToken cancellationToken)
    {
        var partition = await _repository.GetByIdAsync(request.PartitionId, cancellationToken);

        if (partition == null || partition.IsDeleted)
            return Result<string>.Failure("Silinmek istenen partition bulunamadı veya zaten silinmiş.");

        partition.IsDeleted = true;
        partition.UpdatedAt = DateTime.UtcNow;
        _repository.Update(partition);
        await _unitofwork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Partition başarıyla silindi (soft delete).");
    }
}
