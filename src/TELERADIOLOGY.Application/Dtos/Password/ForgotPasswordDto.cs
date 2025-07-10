using System.ComponentModel.DataAnnotations;

namespace TELERADIOLOGY.Application.Dtos.Password;

public class ForgotPasswordDto
{
    public string Email { get; set; } = null!;
}