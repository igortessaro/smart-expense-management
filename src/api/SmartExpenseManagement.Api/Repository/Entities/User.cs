using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartExpenseManagement.Api.Repository.Entities;

public sealed class User
{
    public User(string name, string password)
    {
        Name = name;
        Password = password;
        Uuid = Guid.NewGuid();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
