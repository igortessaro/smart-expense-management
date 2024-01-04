using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartExpenseManagement.Domain.Entities;

public sealed class User
{
    public User(string firstName, string lastName, string login, string password, string role, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Login = login;
        Password = password;
        Role = role;
        Email = email;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
}
