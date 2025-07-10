using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Hospitals.DeleteHospital;

public sealed class DeleteHospitalByIdCommandHandler(
    IHospitalRepository hospitalRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteHospitalByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteHospitalByIdCommand request, CancellationToken cancellationToken)
    {
        var hospital = await hospitalRepository.GetByExpressionAsync(x => x.Id == request.Id, cancellationToken);

        if (hospital is null)
            return Result<string>.Failure("Hastane bulunamadı.");

        if (hospital.IsDeleted)
            return Result<string>.Failure("Hastane daha önce silinmiş.");

        hospital.IsDeleted = true;

        hospitalRepository.Update(hospital);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Hastane başarıyla silindi..");
    }
}
