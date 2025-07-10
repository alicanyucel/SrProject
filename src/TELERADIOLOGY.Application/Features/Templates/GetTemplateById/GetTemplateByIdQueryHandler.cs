using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Application.Features.Templates.GetTemplateById
{
    internal sealed class GetTemplateByIdQueryHandler : IRequestHandler<GetTemplateByIdQuery, Template>
    {
        private readonly ITemplateRepository _templateRepository;

        public GetTemplateByIdQueryHandler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<Template> Handle(GetTemplateByIdQuery request, CancellationToken cancellationToken)
        {
            return await _templateRepository.GetAll().SingleAsync(t => t.Id == request.Id, cancellationToken);
        }
    }
}
