using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Members.DeleteMember;

public sealed class DeleteMemberByIdCommandHandler : IRequestHandler<DeleteMemberByIdCommand, Result<string>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMemberByIdCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<string>> Handle(DeleteMemberByIdCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByExpressionAsync(x => x.Id == request.Id, cancellationToken);

        if (member is null)
            return Result<string>.Failure("Üye bulunamadı.");

        if (member.IsDeleted)
            return Result<string>.Failure("Üye daha önce silinmiş.");

        member.IsDeleted = true;

        _memberRepository.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Üye başarıyla soft silindi.");
    }
}