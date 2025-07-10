using FluentValidation;
using MediatR;
using TELERADIOLOGY.Application.Features.Companies.CreateCompany;
using TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUsersByIdentityNumber;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUser;

public record GetCompanyUsersByIdentityNumberQuery(
    string IdentityNumber)   : IRequest<Result<GetCompanyUsersByIdentityNumberResponse>>;

public class GetCompanyUsersByIdentityNumberQueryValidator : AbstractValidator<GetCompanyUsersByIdentityNumberQuery>
{
    public GetCompanyUsersByIdentityNumberQueryValidator()
    {
        RuleFor(x => x.IdentityNumber)
            .NotEmpty().WithMessage("Tc Kimlik numarası boş olamaz.")
            .Length(11).WithMessage("Tc Kimlik numarası 11 hane olmalıdır");
    }
}
