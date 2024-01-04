using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartExpenseManagement.Domain.Entities;

public sealed class ExpenseGroup
{
    public ExpenseGroup(string description, IReadOnlyList<string> users, string owner)
    {
        Description = description;
        Users = users;
        Owner = owner;
        CreatedAt = DateTime.UtcNow;
        IsDefaultGroup = true;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IReadOnlyList<string> Users { get; set; } = new List<string>();
    public string Owner { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDefaultGroup { get; set; }
}

