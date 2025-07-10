using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Application.Features.Reports.GetReportById;

internal sealed class GetReportByIdQueryHandler : IRequestHandler<GetReportByIdQuery, Report>
{
    private readonly IReportRepository _reportRepository;

    public GetReportByIdQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<Report> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
    {
        return await _reportRepository.GetAll().SingleAsync(r => r.Id == request.Id, cancellationToken);
    }
}