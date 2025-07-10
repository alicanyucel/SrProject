using TELERADIOLOGY.Domain.Abstractions;
using TELERADIOLOGY.Domain.Enums;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class Company : Entity
{
    public string CompanySmallTitle { get; set; }
    public string CompanyTitle { get; set; }
    public string Representative { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string TaxNo { get; set; }
    public string TaxOffice { get; set; }
    public string WebSite { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public bool IsDeleted { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public CompanyType CompanyType { get; set; }
    public ICollection<CompanyUser> CompanyUsers { get; set; }
    public ICollection<Partition> Partitions { get; set; }
}