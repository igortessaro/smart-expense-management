using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartExpenseManagement.Api.Repository.Entities;

public sealed class User
{
    public User(string userName, string password, string role)
    {
        UserName = userName;
        Password = password;
        Role = role;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
