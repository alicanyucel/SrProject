using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Companies.GetAllCompany;


public sealed record GetAllCompanyQuery() : IRequest<Result<List<Company>>>;
