using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Application.Features.Templates.GetAllTemplate;

internal sealed class GetAllTemplateQueryHandler : IRequestHandler<GetAllTemplateQuery, List<Template>>
{
    private readonly ITemplateRepository _templateRepository;

    public GetAllTemplateQueryHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<List<Template>> Handle(GetAllTemplateQuery request, CancellationToken cancellationToken)
    {
        return await _templateRepository.GetAll().OrderBy(p => p.RaporTipi).ToListAsync(cancellationToken);
    }
}