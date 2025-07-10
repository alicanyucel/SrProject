using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Reports.GetReportById;

public sealed record GetReportByIdQuery(Guid Id) : IRequest<Report>;
