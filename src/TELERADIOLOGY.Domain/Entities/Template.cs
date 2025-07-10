using TELERADIOLOGY.Domain.Abstractions;

namespace TELERADIOLOGY.Domain.Entities;

public sealed class Template : Entity
{
    public string Name { get; set; } = string.Empty;
    public string RaporTipi { get; set; } = string.Empty;
    public string ContextHtml { get; set; } = string.Empty;
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public string Content { get; set; } = string.Empty;
}