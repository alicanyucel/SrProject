using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.GetAllDoctorSignature;

internal sealed class GetAllDoctorSignatureQueryHandler(
 ISignatureRepository signatureRepository) : IRequestHandler<GetAllDoctorSignatureQuery, Result<List<DoctorSignature>>>
{
    public async Task<Result<List<DoctorSignature>>> Handle(GetAllDoctorSignatureQuery request, CancellationToken cancellationToken)
    {
        List<DoctorSignature> doctorSignature = await signatureRepository.GetAll().OrderBy(p => p.DisplayName).ToListAsync();
        return doctorSignature;
    }
}
