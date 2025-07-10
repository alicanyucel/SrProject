using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Templates.UpdateTemplate;

internal sealed class UpdateTemplateByIdCommandHandler(ITemplateRepository templateRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateTemplateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        Template  template = await templateRepository.GetByExpressionWithTrackingAsync(P => P.Id == request.Id, cancellationToken);
        if (template == null)
        {
            return Result<string>.Failure("Şablon yok");
        }
        mapper.Map(request, template);
        templateRepository.Update(template);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Şablon güncellendi.";

    }
}