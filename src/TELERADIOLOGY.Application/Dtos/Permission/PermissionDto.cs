namespace TELERADIOLOGY.Application.Dtos.Permission;

public class PermissionDto
{
    public int Id { get; set; }
    public string EndPoint { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
