namespace TELERADIOLOGY.Application.Dtos.Company;

public class CompanyResultDto
{
    public string CompanyTitle { get; set; }= string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }=string.Empty;
    public string CompanySmallTitle {  get; set; }= string.Empty;
    public bool IsActive { get; set; }
}
