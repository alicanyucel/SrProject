using MediatR;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Features.Companies.GetCompanyById;

public sealed record GetCompanyByIdQuery(Guid Id) : IRequest<Company>;