using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Reports.GetAllReport;

public sealed record GetAllReportQuery() : IRequest<List<Report>>;
