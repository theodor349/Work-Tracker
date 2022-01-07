namespace DataAccess.Internal;

public abstract class GenericRepository<T, I> : IGenericRepository<T, I> where T : class
{
    internal readonly ISqlDataAccess _sql;
    internal readonly string _tableName;

    internal StoredProcedure _create;
    internal StoredProcedure _update;
    internal StoredProcedure _delete;

    public GenericRepository(ISqlDataAccess sqlDataAccess, string tableName, StoredProcedure create, StoredProcedure update, StoredProcedure delete)
    {
        _sql = sqlDataAccess;
        _tableName = tableName;
        _create = create;
        _update = update;
        _delete = delete;
    }

    internal abstract bool Compare(T model, I id);

    public virtual Task<int> CreateAsync(T entity)
    {
        var data = _sql.SaveDataInTransaction(_create, entity);
        return Task.FromResult(data);
    }

    public virtual Task<int> DeleteAsync(I id)
    {
        var data = _sql.DeleteDataInTransaction(_delete, new { id });
        return Task.FromResult(data);
    }

    public virtual Task<IReadOnlyList<T>> GetAllAsync()
    {
        var data = _sql.LoadDataInTransaction<T, object>(StoredProcedure.spGeneric_GetAll, new { table = _tableName });
        return Task.FromResult(data);
    }

    public virtual async Task<T> GetByIdAsync(I id)
    {
        var data = await GetAllAsync();
        return data.FirstOrDefault(x => Compare(x, id));
    }

    public virtual Task<int> UpdateAsync(T entity)
    {
        var data = _sql.UpdateDataInTransaction(_update, entity);
        return Task.FromResult(data);
    }

    public void StartTransaction()
    {
        _sql.StartTransaction();
    }

    public void CommitTransaction()
    {
        _sql.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        _sql.RollbackTransaction();
    }
}
