using TELERADIOLOGY.Domain.Abstractions;
using TELERADIOLOGY.Domain.Enums;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class Member : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
    public string FullName { get; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsDeleted { get; set; }
    public ApplicationStatus ApplicationStatus { get; set; }
    public AreaOfInterest AreaOfInterest { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string AddInfoMessage { get; set; } // Added missing property  
}