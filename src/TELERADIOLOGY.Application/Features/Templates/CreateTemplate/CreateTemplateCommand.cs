using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Templates.CreateTemplate;
public record CreateTemplateCommand(
    string Name,
    string RaporTipi,
    string ContextHtml,
    string Content
) : IRequest<Result<string>>;
