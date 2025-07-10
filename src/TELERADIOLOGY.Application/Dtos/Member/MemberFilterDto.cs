using TELERADIOLOGY.Domain.Enums;

namespace TELERADIOLOGY.Application.Dtos.MemberDtos;
public class MemberFilterDto
{
    public string SearchTerm { get; set; } = default!;
    public ApplicationStatus ApplicationStatus { get; set; } = default!;
}
