using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.DoctorSignatures.DeleteDoctorSignature;
public sealed class DeleteDoctorSignatureByIdCommandHandler : IRequestHandler<DeleteDoctorSignatureByIdCommand, Result<string>>
{
    private readonly ISignatureRepository _signatureRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDoctorSignatureByIdCommandHandler(ISignatureRepository signatureRepository, IUnitOfWork unitOfWork)
    {
        _signatureRepository = signatureRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(DeleteDoctorSignatureByIdCommand request, CancellationToken cancellationToken)
    {
        var signature = await _signatureRepository.GetByExpressionAsync(x => x.Id == request.SignatureId, cancellationToken);

        if (signature is null)
            return Result<string>.Failure("İmza bulunamadı.");

        if (signature.IsDeleted)
            return Result<string>.Failure("Signature daha önce silinmiş.");

        signature.IsDeleted = true;

        _signatureRepository.Update(signature);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Doktor imzası başarıyla soft silindi.");
    }
    // bütün sorunlar çözüldü
}