using MediatR;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.Partitions.CreatePartition;

public sealed record CreatePartitionCommand(
    Guid CompanyId,
    Guid HospitalId,
    string PartitionName,
    bool IsActive,
    bool Urgent,
    string Modality,
    string ReferenceKey,
    string PartitionCode,
    string CompanyCode
) : IRequest<Result<string>>;

