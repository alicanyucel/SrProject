using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Hospitals.CreateHospital;
// diğer apiler yazılacak postgresqle cekilecek
internal sealed class CreateHospitalCommandHandler(
    IHospitalRepository hospitalRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateHospitalCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateHospitalCommand request, CancellationToken cancellationToken)
    {
        bool isTaxNumberExists = await hospitalRepository.AnyAsync(
            c => c.TaxNumber == request.TaxNumber, cancellationToken);

        if (isTaxNumberExists)
            return Result<string>.Failure("Bu vergi numarası ile daha önce kayıt oluşturulmuş.");

        Hospital hospital = mapper.Map<Hospital>(request);

        hospitalRepository.Add(hospital);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Hastane başarıyla kaydedildi.";
    }
}