using System.ComponentModel.DataAnnotations;

namespace SmartExpenseManagement.Domain.CQRS.Commands;

public sealed class LoginCommand
{
    [Required]
    public string Login { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

