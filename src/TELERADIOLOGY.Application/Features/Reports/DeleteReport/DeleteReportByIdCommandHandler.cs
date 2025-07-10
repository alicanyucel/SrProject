using GenericRepository;
using MediatR;
using TELERADIOLOGY.Application.Features.Companies.DeleteCompany;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Reports.DeleteReport
{
    public sealed class DeleteReportByIdCommandHandler : IRequestHandler<DeleteReportByIdCommand, Result<string>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteReportByIdCommandHandler(IReportRepository reportRepository, IUnitOfWork unitOfWork)
        {
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteReportByIdCommand request, CancellationToken cancellationToken)
        {
            var report = await _reportRepository.GetByExpressionAsync(x => x.Id == request.Id, cancellationToken);

            if (report is null)
                return Result<string>.Failure("Rapor bulunamadı.");

            if (report.IsDeleted)
                return Result<string>.Failure("Rapor daha önce silinmiş.");

            report.IsDeleted = true;

            _reportRepository.Update(report);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Rapor başarıyla soft silindi.");
        }
    }
}