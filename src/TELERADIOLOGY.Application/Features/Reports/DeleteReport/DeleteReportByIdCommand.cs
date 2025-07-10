using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Reports.DeleteReport;


public sealed record DeleteReportByIdCommand(Guid Id) : IRequest<Result<string>>;
