namespace SmartExpenseManagement.Api.Commands;

public sealed class CreateUserCommand
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
