using System.ComponentModel.DataAnnotations;

namespace SmartExpenseManagement.Domain.CQRS.Commands;

public sealed class UpdateExpenseCommand
{
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Value { get; set; }
    [Required]
    public string Category { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public DateTime? PaydAt { get; set; }
}
