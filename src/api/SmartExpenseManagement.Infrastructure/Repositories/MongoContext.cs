using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartExpenseManagement.Domain.Options;

namespace SmartExpenseManagement.Infrastructure.Repositories;

public sealed class MongoContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    public MongoContext(IOptionsMonitor<DatabaseSettings> dbOptions)
    {
        var settings = dbOptions.CurrentValue;

        if (string.IsNullOrEmpty(settings.ConnectionString))
        {
            throw new ArgumentNullException(nameof(settings.ConnectionString));
        }

        if (string.IsNullOrEmpty(settings.DatabaseName))
        {
            throw new ArgumentNullException(nameof(settings.DatabaseName));
        }

        _client = new MongoClient(settings.ConnectionString);
        _database = _client.GetDatabase(settings.DatabaseName);
    }

    public IMongoClient Client => _client;

    public IMongoDatabase Database => _database;
}
