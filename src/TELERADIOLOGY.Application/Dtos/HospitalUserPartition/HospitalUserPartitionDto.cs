namespace TELERADIOLOGY.Application.Dtos.HospitalUserPartition;

public sealed class HospitalUserPartitionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string PartitionName { get; set; }
    public bool Urgent { get; set; }
    public required string Modality { get; set; }
    public required string ReferenceKey { get; set; }
    public required string PartitionCode { get; set; }
    public required string CompanyCode { get; set; }
    public Guid CompanyId { get; set; }
    public Guid HospitalId { get; set; }
}
