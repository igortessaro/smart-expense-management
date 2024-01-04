using System.ComponentModel.DataAnnotations;

namespace SmartExpenseManagement.Domain.CQRS.Commands;

public sealed class CreateUserCommand
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string Login { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Role { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
}
