using SmartExpenseManagement.Api.Repository.Entities;
using MongoDB.Driver;

namespace SmartExpenseManagement.Api.Repository;

public sealed class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(MongoContext dbContext) : base(dbContext, MongoCollectionNames.Users) { }


    public override async Task<User> UpdateAsync(User obj)
    {
        var filter = Builders<User>.Filter.Where(x => x.Id == obj.Id);

        var updateDefBuilder = Builders<User>.Update;
        var updateDef = updateDefBuilder.Combine(new UpdateDefinition<User>[]
        {
                updateDefBuilder.Set(x => x.UserName, obj.UserName),
                updateDefBuilder.Set(x => x.Password, obj.Password)
        });
        await this._collection.FindOneAndUpdateAsync(filter, updateDef);

        return await this._collection.FindOneAndReplaceAsync(x => x.Id == obj.Id, obj);
    }
}
