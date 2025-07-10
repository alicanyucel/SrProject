namespace TELERADIOLOGY.Application.Dtos.Hospital;

public class HospitalResultDto
{
    public string ShortName { get; set; } = default!;
    public string FullTitle { get; set; } = default!;
    public string AuthorizedPerson { get; set; } = default!;
    public string City { get; set; } = default!;
    public string District { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string TaxNumber { get; set; } = default!;
    public string TaxOffice { get; set; } = default!;
    public string Website { get; set; } = default!;
    public bool IsActive { get; set; }

}
