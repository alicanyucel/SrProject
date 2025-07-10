using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Reports.CreateReport;

public sealed record CreateReportCommand(
   string ReportName,
   bool Emergency,
   string ModalityType,
   Guid TemplateId
    ) : IRequest<Result<string>>;