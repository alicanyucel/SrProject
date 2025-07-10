using AutoMapper;
using GenericRepository;
using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Templates.CreateTemplate;



internal sealed class CreateTemplateCommandHandler(ITemplateRepository templateRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateTemplateCommand, Result<string>>
{
    public ITemplateRepository TemplateRepository { get; } = templateRepository;

    public async Task<Result<string>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        Template template = mapper.Map<Template>(request);
        await TemplateRepository.AddAsync(template, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Sablon kaydı yapıldı.";
    }

}
