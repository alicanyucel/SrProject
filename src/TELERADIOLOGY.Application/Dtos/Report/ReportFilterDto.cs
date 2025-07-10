namespace TELERADIOLOGY.Application.Dtos.ReportDto;

public class ReportTypeFilterDto
{
    public string ModalityType { get; set; } = default!;
    public bool Emergency { get; set; }
    public string ReportName { get; set; } = default!;
}

