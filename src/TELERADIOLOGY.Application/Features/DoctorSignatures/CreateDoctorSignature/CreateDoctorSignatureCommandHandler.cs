using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.CreateDoctorSignature;

internal sealed class CreateDoctorSignatureCommandHandler(
    ISignatureRepository doctorSignatureRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateDoctorSignatureCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateDoctorSignatureCommand request, CancellationToken cancellationToken)
    {
        bool isExists = await doctorSignatureRepository.AnyAsync(
            ds => ds.RegisterNo == request.RegisterNo,
            cancellationToken
        );

        if (isExists)
            return Result<string>.Failure("Bu sicil numarası ile daha önce kayıt yapılmış.");

        DoctorSignature doctorSignature = mapper.Map<DoctorSignature>(request);

        doctorSignatureRepository.Add(doctorSignature);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Doktor imzası başarıyla kaydedildi.");
    }
}
