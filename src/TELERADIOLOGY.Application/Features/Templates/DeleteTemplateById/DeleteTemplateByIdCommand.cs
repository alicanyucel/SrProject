using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Templates.DeleteTemplateById;


public sealed record DeleteTemplateByIdCommand(Guid Id) : IRequest<Result<string>>;