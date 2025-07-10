using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Templates.GetAllTemplate;

public sealed record GetAllTemplateQuery() : IRequest<List<Template>>;
