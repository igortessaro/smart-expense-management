using SmartExpenseManagement.Api.Repository.Entities;
using MongoDB.Driver;

namespace SmartExpenseManagement.Api.Repository;

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
        await this._collection.FindOneAndUpdateAsync(filter, updateDef);

        return await this._collection.FindOneAndReplaceAsync(x => x.Id == obj.Id, obj);
    }
}
