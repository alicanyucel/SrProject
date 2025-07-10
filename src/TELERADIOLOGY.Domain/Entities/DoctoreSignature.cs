using TELERADIOLOGY.Domain.Abstractions;

namespace TELERADIOLOGY.Domain.Entities;
public sealed class DoctorSignature : Entity
{
    public required string Degree { get; set; }    
    public required string DegreeNo { get; set; }
    public required string DiplomaNo { get; set; } 
    public required string RegisterNo { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string DisplayName { get; set; } = string.Empty; 
    public required byte[] Signature { get; set; } 
}
