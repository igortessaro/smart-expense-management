using System.ComponentModel.DataAnnotations;

namespace SmartExpenseManagement.Domain.CQRS.Commands;

public sealed class CreateExpenseGroupCommand
{
    [Required]
    public string Description { get; set; } = string.Empty;
    public IReadOnlyList<string> Users { get; set; } = new List<string>();
    public bool DefaultGroup { get; set; }
}

