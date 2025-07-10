
public class CompanyUserDto
{
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid CompanyId { get; set; }
    public required string CompanyTitle { get; set; }
    public required string CompanySmallTitle { get; set; }
    public required string Representative { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }  
}
