using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Companies.DeleteCompany
{
    public sealed record DeleteCompanyByIdCommand(Guid CompanyId) : IRequest<Result<string>>;
}
