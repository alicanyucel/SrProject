using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Reports.UpdateReport;

public sealed record UpdateReportCommand(
   Guid Id,
   string ReportName,
   bool Emergency,
   string ModalityType
) : IRequest<Result<string>>;