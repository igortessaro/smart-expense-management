using System.Linq.Expressions;

namespace SmartExpenseManagement.Domain.Repositories;

public interface IRepository<T>
{
    IQueryable<T> GetAll();

    Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

    Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);

    Task AddAsync(T obj);

    Task<T> UpdateAsync(T obj);

    Task DeleteAsync(Expression<Func<T, bool>> predicate);
}
