namespace DataAccess.Internal;

public interface IGenericRepository<T, I> where T : class
{
    Task<T> GetByIdAsync(I id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<int> CreateAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(I id);

    void StartTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
