using System.ComponentModel.DataAnnotations;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class Permission
{
    [Key]
    public int Id { get; set; }
    public string EndPoint { get; set; }
    public string Method { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; } = false; 
}
