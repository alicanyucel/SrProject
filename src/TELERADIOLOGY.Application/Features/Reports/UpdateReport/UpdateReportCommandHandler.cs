using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Reports.UpdateReport;

internal sealed class UpdateReportByIdCommandHandler(IReportRepository reportRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateReportCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        Report report = await reportRepository.GetByExpressionWithTrackingAsync(P => P.Id == request.Id, cancellationToken);
        if (report == null)
        {
            return Result<string>.Failure("rapor yok");
        }
        mapper.Map(request, report);
        reportRepository.Update(report);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Rapor güncellendi.";
    }
}