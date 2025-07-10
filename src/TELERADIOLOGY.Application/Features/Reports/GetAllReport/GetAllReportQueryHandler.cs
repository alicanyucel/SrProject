using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Application.Features.Reports.GetAllReport;

internal sealed class GetAllReportQueryHandler : IRequestHandler<GetAllReportQuery, List<Report>>
{
    private readonly IReportRepository _reportRepository;

    public GetAllReportQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<List<Report>> Handle(GetAllReportQuery request, CancellationToken cancellationToken)
    {
        return await _reportRepository
            .GetAll()
            .Where(p => !p.IsDeleted)
            .OrderBy(p => p.ReportName)
            .ToListAsync(cancellationToken);
    }
}
