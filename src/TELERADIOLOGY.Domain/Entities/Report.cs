using TELERADIOLOGY.Domain.Abstractions;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class Report : Entity
{
    public string ReportName { get; set; } = string.Empty;
    public bool Emergency { get; set; }
    public string ModalityType { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public Template RaporSablonu { get; set; } = null!;
}
