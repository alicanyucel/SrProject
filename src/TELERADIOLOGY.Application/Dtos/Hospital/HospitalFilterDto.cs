namespace TELERADIOLOGY.Application.Dtos.HospitalDto;

public class HospitalFilterDto
{
    public string SearchTerm { get; set; } = default!;
    public bool IsActive { get; set; }
}