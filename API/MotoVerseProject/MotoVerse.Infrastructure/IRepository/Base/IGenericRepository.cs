using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace MotoVerse.Infrastructure.IRepository.Base;

public interface IGenericRepository<T>
{
    // -------------------- Create
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(ICollection<T> entities);

    // -------------------- Read
    Task<bool> IsNameExistExceptSelf(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(string id);
    Task<int> CountAsync();

    IQueryable<T> GetTableNoTracking();
    IQueryable<T> GetTableAsTracking();
    Task<T> GetByIdWithIncludeAsync(string id, params Expression<Func<T, object>>[] includes);
    Task<T> GetByConditionAsync(string id, params Expression<Func<T, bool>>[] conditions);

    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

    // -------------------- Update
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(ICollection<T> entities);

    // -------------------- Delete
    Task DeleteRangeAsync(ICollection<T> entities);
    Task DeleteAsync(T entity);

    // -------------------- Save Changes 
    Task SaveChangesAsync();

    // -------------------- Transaction
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitAsync();
    Task RollBackAsync();

}


