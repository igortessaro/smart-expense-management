using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartExpenseManagement.Domain.Options;

namespace SmartExpenseManagement.Infrastructure.Repositories;

public sealed class MongoContext
{
    private readonly ILogger<MongoContext> _logger;
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    public MongoContext(IOptionsMonitor<DatabaseSettings> dbOptions, ILogger<MongoContext> logger)
    {
        _logger = logger;
        _logger.LogInformation("Database Settings: {Connection} and {Database}", dbOptions.CurrentValue.ConnectionString, dbOptions.CurrentValue.DatabaseName);
        var settings = dbOptions.CurrentValue;

        _client = new MongoClient(settings.ConnectionString);
        _database = _client.GetDatabase(settings.DatabaseName);
    }

    public IMongoClient Client => _client;

    public IMongoDatabase Database => _database;
}
