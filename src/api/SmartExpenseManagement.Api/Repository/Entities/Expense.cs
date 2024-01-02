using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartExpenseManagement.Api.Repository.Entities;

public sealed class Expense
{
    public Expense(string userId, string description, decimal value)
    {
        UserId = userId;
        Description = description;
        Value = value;
        CreatedAt = DateTime.UtcNow;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
}
