using SmartExpenseManagement.Api.Repository.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace SmartExpenseManagement.Api.Repository;

public abstract class BaseRepository<T> : IRepository<T>
{
    public readonly IMongoCollection<T> _collection;

    public BaseRepository(MongoContext dbContext, string colletionName)
    {
        this._collection = dbContext.Database.GetCollection<T>(colletionName);
    }

    public async Task AddAsync(T obj)
    {
        await this._collection.InsertOneAsync(obj);
    }

    public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        _ = await _collection.DeleteOneAsync(predicate);
    }

    public IQueryable<T> GetAll()
    {
        return this._collection.AsQueryable();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        var filter = Builders<T>.Filter.Where(predicate);
        return (await this._collection.FindAsync(filter)).ToList();
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
    {
        var filter = Builders<T>.Filter.Where(predicate);
        return (await this._collection.FindAsync(filter)).FirstOrDefault();
    }

    public abstract Task<T> UpdateAsync(T obj);
}
