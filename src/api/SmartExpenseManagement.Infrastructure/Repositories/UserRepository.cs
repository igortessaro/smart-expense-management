using MongoDB.Driver;
using SmartExpenseManagement.Domain.Entities;
using SmartExpenseManagement.Domain.Repositories;

namespace SmartExpenseManagement.Infrastructure.Repositories;

public sealed class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(MongoContext dbContext) : base(dbContext, MongoCollectionNames.Users) { }


    public override async Task<User> UpdateAsync(User obj)
    {
        var filter = Builders<User>.Filter.Where(x => x.Id == obj.Id);

        var updateDefBuilder = Builders<User>.Update;
        var updateDef = updateDefBuilder.Combine(new UpdateDefinition<User>[]
        {
                updateDefBuilder.Set(x => x.Login, obj.Login),
                updateDefBuilder.Set(x => x.Password, obj.Password)
        });
        await _collection.FindOneAndUpdateAsync(filter, updateDef);

        return await _collection.FindOneAndReplaceAsync(x => x.Id == obj.Id, obj);
    }
}
