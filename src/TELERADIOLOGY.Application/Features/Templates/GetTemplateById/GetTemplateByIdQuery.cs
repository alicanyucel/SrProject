using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Templates.GetTemplateById;

public sealed record GetTemplateByIdQuery(Guid Id) : IRequest<Template>;