using MongoDB.Driver;
using SmartExpenseManagement.Domain.Entities;
using SmartExpenseManagement.Domain.Repositories;

namespace SmartExpenseManagement.Infrastructure.Repositories;

public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
{
    public ExpenseRepository(MongoContext dbContext) : base(dbContext, MongoCollectionNames.Expenses) { }

    public override async Task<Expense> UpdateAsync(Expense obj)
    {
        var filter = Builders<Expense>.Filter.Where(x => x.Id == obj.Id);

        var updateDefBuilder = Builders<Expense>.Update;
        var updateDef = updateDefBuilder.Combine(new UpdateDefinition<Expense>[]
        {
                updateDefBuilder.Set(x => x.Description, obj.Description),
                updateDefBuilder.Set(x => x.Value, obj.Value)
        });
        await _collection.FindOneAndUpdateAsync(filter, updateDef);

        return await _collection.FindOneAndReplaceAsync(x => x.Id == obj.Id, obj);
    }
}
