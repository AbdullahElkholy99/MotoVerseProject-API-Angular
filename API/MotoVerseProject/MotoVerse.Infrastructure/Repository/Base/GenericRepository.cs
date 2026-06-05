using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace MotoVerse.Infrastructure.Repository.Base;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{

    #region Vars / Props

    protected readonly AppDbContext _dbContext;

    #endregion

    #region Constructor
    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    #region Methods

    // -------------------- Create
    public virtual async Task AddRangeAsync(ICollection<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);

    }
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);

        return entity;
    }
    // -------------------- Read
    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>()
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<bool> IsNameExistExceptSelf(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().AnyAsync(predicate);
    }

    public async Task<int> CountAsync()
    {
        return await _dbContext.Set<T>().CountAsync();
    }
    public virtual async Task<T> GetByIdAsync(string id)
    {

        return await _dbContext.Set<T>().FindAsync(id);
    }
    public IQueryable<T> GetTableNoTracking()
    {
        return _dbContext.Set<T>().AsNoTracking().AsQueryable();
    }
    public async Task<T> GetByIdWithIncludeAsync(string id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        foreach (var include in includes)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync(e =>
            EF.Property<string>(e, "Id") == id);
    }
    public async Task<T> GetByConditionAsync(string id, params Expression<Func<T, bool>>[] conditions)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        foreach (var condition in conditions)
            query = query.Where(condition);

        return await query.FirstOrDefaultAsync(e =>
            EF.Property<string>(e, "Id") == id);
    }
    public IQueryable<T> GetTableAsTracking()
    {
        return _dbContext.Set<T>().AsQueryable();
    }
    // -------------------- Update
    public virtual async Task UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        await Task.CompletedTask;
    }
    public virtual async Task UpdateRangeAsync(ICollection<T> entities)
    {
        _dbContext.Set<T>().UpdateRange(entities);
        await Task.CompletedTask;
    }

    // -------------------- Delete
    public virtual async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }
    public virtual async Task DeleteRangeAsync(ICollection<T> entities)
    {
        foreach (var entity in entities)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }
        await Task.CompletedTask;
    }

    // -------------------- Save Changes
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    // -------------------- Transaction
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _dbContext.Database.CommitTransactionAsync();
    }

    public async Task RollBackAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }






    #endregion
}
