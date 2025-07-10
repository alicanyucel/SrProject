using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Hospitals.UpdateHospital;

internal sealed class UpdateHospitalCommandHandler(
    IHospitalRepository hospitalRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateHospitalCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateHospitalCommand request, CancellationToken cancellationToken)
    {
        Hospital hospital = await hospitalRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (hospital is null)
        {
            return Result<string>.Failure("Bu id'ye uygun kayıt bulunamadı");
        }

        if(hospital.TaxNumber != request.TaxNumber)
        {
            bool isTaxNumberExists = await hospitalRepository.AnyAsync(p => p.TaxNumber == request.TaxNumber, cancellationToken);

            if (isTaxNumberExists)
            {
                return Result<string>.Failure("Bu vergi numarası ile daha önce kayıt oluşturulmuş");
            }
        }

        mapper.Map(request, hospital);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Hastane kaydı başarıyla güncellendi";
    }
}
