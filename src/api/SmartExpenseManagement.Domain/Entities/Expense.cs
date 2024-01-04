using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartExpenseManagement.Domain.Entities;

public sealed class Expense
{
    public Expense(string userId, string description, decimal? value, string category, DateTime? dueDate, DateTime? paydAt, string period, string expenseGroupId)
    {
        UserId = userId;
        Description = description;
        Value = value;
        CreatedAt = DateTime.UtcNow;
        Category = category;
        DueDate = dueDate;
        PaydAt = paydAt;
        Period = period;
        ExpenseGroupId = expenseGroupId;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; }
    public string Description { get; set; }
    public decimal? Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Category { get; set; }
    public string Period { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? PaydAt { get; set; }
    public string ExpenseGroupId { get; set; }
}
