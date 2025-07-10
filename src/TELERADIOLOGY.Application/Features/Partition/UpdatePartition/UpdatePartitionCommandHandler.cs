using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Extensions;
using TELERADIOLOGY.Domain.Repositories;

public class UpdatePartitionCommandHandler : IRequestHandler<UpdatePartitionCommand, string>
{
    private readonly IPartitionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdatePartitionCommandHandler(IPartitionRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(UpdatePartitionCommand request, CancellationToken cancellationToken)
    {
        var partition = await _repository.GetByIdAsync(request.PartitionId);

        if (partition == null || partition.IsDeleted)
            return "partition yok.";

        partition.CompanyId = request.CompanyId;
        partition.HospitalId = request.HospitalId;
        partition.PartitionName = request.PartitionName;
        partition.IsActive = request.IsActive;
        partition.Urgent = request.Urgent;
        partition.Modality = request.Modality;
        partition.ReferenceKey = request.ReferenceKey;
        partition.PartitionCode = request.PartitionCode;
        partition.CompanyCode = request.CompanyCode;
        partition.UpdatedAt = DateTime.UtcNow;

        _repository.Update(partition);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return "Partition updated successfully.";
    }
}
