using MongoDB.Driver;
using SmartExpenseManagement.Domain.Entities;
using SmartExpenseManagement.Domain.Repositories;

namespace SmartExpenseManagement.Infrastructure.Repositories;

public sealed class ExpenseGroupRepository : BaseRepository<ExpenseGroup>, IExpenseGroupRepository
{
    public ExpenseGroupRepository(MongoContext dbContext) : base(dbContext, MongoCollectionNames.ExpenseGroups) { }

    public override async Task<ExpenseGroup> UpdateAsync(ExpenseGroup obj)
    {
        var filter = Builders<ExpenseGroup>.Filter.Where(x => x.Id == obj.Id);

        var updateDefBuilder = Builders<ExpenseGroup>.Update;
        var updateDef = updateDefBuilder.Combine(new UpdateDefinition<ExpenseGroup>[]
        {
                updateDefBuilder.Set(x => x.Description, obj.Description),
                updateDefBuilder.Set(x => x.Users, obj.Users)
        });
        await _collection.FindOneAndUpdateAsync(filter, updateDef);

        return await _collection.FindOneAndReplaceAsync(x => x.Id == obj.Id, obj);
    }
}
