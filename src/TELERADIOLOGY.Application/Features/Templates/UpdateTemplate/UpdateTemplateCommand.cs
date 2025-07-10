using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Templates.UpdateTemplate;

public record UpdateTemplateCommand(
 Guid Id, 
 string Name,
 string RaporTipi,
 string ContextHtml,
 string Content
) : IRequest<Result<string>>;
