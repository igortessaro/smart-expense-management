using MongoDB.Driver;
using SmartExpenseManagement.Domain.Repositories;
using System.Linq.Expressions;

namespace SmartExpenseManagement.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IRepository<T>
{
    public readonly IMongoCollection<T> _collection;

    public BaseRepository(MongoContext dbContext, string colletionName)
    {
        _collection = dbContext.Database.GetCollection<T>(colletionName);
    }

    public async Task AddAsync(T obj)
    {
        await _collection.InsertOneAsync(obj);
    }

    public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        _ = await _collection.DeleteOneAsync(predicate);
    }

    public IQueryable<T> GetAll()
    {
        return _collection.AsQueryable();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        var filter = Builders<T>.Filter.Where(predicate);
        return (await _collection.FindAsync(filter)).ToList();
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
    {
        var filter = Builders<T>.Filter.Where(predicate);
        return (await _collection.FindAsync(filter)).FirstOrDefault();
    }

    public abstract Task<T> UpdateAsync(T obj);
}
