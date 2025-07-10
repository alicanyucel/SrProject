namespace TELERADIOLOGY.Application.Dtos.Company;

public class CompanyFilterDto
{
    public string SearchTerm { get; set; } = default!;
    public bool IsActive { get; set; }
}
