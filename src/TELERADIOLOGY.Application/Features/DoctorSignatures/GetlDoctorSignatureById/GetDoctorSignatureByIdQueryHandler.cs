using MediatR;
using TELERADIOLOGY.Application.Features.DoctorSignatures.GetAllDoctorSignatureById;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.GetlDoctorSignatureById;

internal sealed class GetDoctorSignatureByIdQueryHandler(
    ISignatureRepository signatureRepository) : IRequestHandler<GetDoctorSignatureByIdQuery, Result<DoctorSignature>>
{
    public async Task<Result<DoctorSignature>> Handle(GetDoctorSignatureByIdQuery request, CancellationToken cancellationToken)
    {
        var signature = await signatureRepository.GetByExpressionAsync(x => x.Id == request.Id, cancellationToken);

        if (signature is null)
        {
            return Result<DoctorSignature>.Failure("İmza Bulunamadı");
        }

        if (signature.IsDeleted)
        {
            return Result<DoctorSignature>.Failure("İmza daha önce silinmiş.");
        }

        return signature;
    }
}