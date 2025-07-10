
using TELERADIOLOGY.Domain.Abstractions;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class Partition:Entity
{
    public Guid CompanyId { get; set; }
    public Guid HospitalId { get; set; }
    public string PartitionName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Urgent { get; set; }
    public string Modality { get; set; }
    public string ReferenceKey { get; set; }
    public string PartitionCode { get; set; }
    public string CompanyCode { get; set; }
    public bool IsDeleted { get; set; } = false;
}
