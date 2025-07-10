using MediatR;
using Microsoft.EntityFrameworkCore;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

namespace TELERADIOLOGY.Application.Features.Companies.GetCompanyById;

internal sealed class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Company>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyByIdQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<Company> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _companyRepository.GetAll().SingleAsync(c => c.Id == request.Id, cancellationToken);
    }
}
