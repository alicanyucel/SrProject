using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Features.Hospitals.UpdateHospital;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.UpdateDoctorSignature;

internal sealed class UpdateDoctorSignatureByIdCommandHandler(
 ISignatureRepository signatureRepository,
 IMapper mapper,
 IUnitOfWork unitOfWork) : IRequestHandler<UpdateDoctorSignatureByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateDoctorSignatureByIdCommand request, CancellationToken cancellationToken)
    {
        DoctorSignature signature = await signatureRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (signature is null)
        {
            return Result<string>.Failure("Bu idye uygun kayıt bulunamadı");
        }

        if (signature.DiplomaNo != request.DiplomaNo)
        {
            bool isDiplomaNo = await signatureRepository.AnyAsync(p => p.DiplomaNo == request.DiplomaNo, cancellationToken);

            if (isDiplomaNo)
            {
                return Result<string>.Failure("Bu diploma nosu daha önce olusturulmuş");
            }
        }

        mapper.Map(request, signature);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Doktor imzası başarıyla güncellendi";
    }
}