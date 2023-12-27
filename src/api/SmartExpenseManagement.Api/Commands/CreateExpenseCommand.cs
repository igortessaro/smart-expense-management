using System.ComponentModel.DataAnnotations;

namespace SmartExpenseManagement.Api.Commands;

public sealed class CreateExpenseCommand
{
    [Required]
    public Guid UserUuid { get; set; }
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Value { get; set; }
}
