using MediatR;

public record UpdatePartitionCommand(
    Guid PartitionId,
    Guid CompanyId,
    Guid HospitalId,
    string PartitionName,
    bool IsActive,
    bool Urgent,
    string Modality,
    string ReferenceKey,
    string PartitionCode,
    string CompanyCode
) : IRequest<string>;

