using TELERADIOLOGY.Domain.Abstractions;

namespace TELERADIOLOGY.Domain.Entities;
public sealed class Hospital : Entity
{
    public string ShortName { get; set; } = default!; 
    public string FullTitle { get; set; } = default!;
    public string AuthorizedPerson { get; set; } = default!;
    public string City { get; set; } = default!; 
    public string District { get; set; } = default!;
    public string Phone { get; set; } 
    public string Email { get; set; } = default!;
    public string Address { get; set; } = default!; 
    public string TaxNumber { get; set; } = default!; 
    public string TaxOffice { get; set; } = default!; 
    public string Website { get; set; } = default!;
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } 
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public ICollection<Partition> Partitions { get; set; } = new List<Partition>();
}