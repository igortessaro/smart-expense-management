using System.ComponentModel.DataAnnotations;

namespace SmartExpenseManagement.Api.Commands;

public sealed class CreateUserCommand
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Role { get; set; } = string.Empty;
}
