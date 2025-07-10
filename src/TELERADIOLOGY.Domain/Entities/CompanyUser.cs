using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class CompanyUser
{
    [Key, Column(Order = 0)]
    public Guid CompanyId { get; set; }
    [Key, Column(Order = 1)]
    public Guid UserId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    [ForeignKey(nameof(CompanyId))]
    [JsonIgnore]
    public Company Company { get; set; }
    [ForeignKey(nameof(UserId))]
    [JsonIgnore]
    public AppUser User { get; set; }
    public bool IsDeleted { get; set; } = false; //soft delete
}
