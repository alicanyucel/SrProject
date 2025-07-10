using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Reports.CreateReport;


internal sealed class CreateReportCommandHandler(IReportRepository reportRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateReportCommand, Result<string>>
{
    public IReportRepository ReportRepository { get; } = reportRepository;

    public async Task<Result<string>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        Report report = mapper.Map<Report>(request);
        await ReportRepository.AddAsync(report, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Rapor kaydı yapıldı.";
    }
}