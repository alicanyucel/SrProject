using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Hospitals.CreateHospital;
public sealed record CreateHospitalCommand(
    string ShortName,
    string FullTitle,
    string AuthorizedPerson,
    string City,
    string District,
    string Phone,
    string Email,
    string Address,
    string TaxNumber,
    string TaxOffice,
    string Website,
    bool IsActive) : IRequest<Result<string>>;