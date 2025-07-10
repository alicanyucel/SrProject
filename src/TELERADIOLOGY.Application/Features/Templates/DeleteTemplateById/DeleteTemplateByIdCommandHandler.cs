using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Templates.DeleteTemplateById;

internal sealed class DeleteTemplateByIdCommandHandler(
ITemplateRepository templateRepository, IUnitOfWork
unitOfWork) : IRequestHandler<DeleteTemplateByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteTemplateByIdCommand request, CancellationToken cancellationToken)
    {
        Template template = await templateRepository.GetByExpressionAsync(t => t.Id == request.Id, cancellationToken);
        if (template is null)
        {
            return Result<string>.Failure("Sablon bulunamadi");
        }

        templateRepository.Delete(template);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Sablon Başarıyla Silindi.";
    }
}