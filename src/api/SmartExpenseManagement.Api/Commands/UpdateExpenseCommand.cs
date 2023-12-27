using System.ComponentModel.DataAnnotations;

namespace SmartExpenseManagement.Api.Commands;

public sealed class UpdateExpenseCommand
{
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Value { get; set; }
}
