namespace DataAccess.Internal;

public interface ISqlDataAccess
{
    void StartTransaction();
    void CommitTransaction();
    void RollbackTransaction();
    void Dispose();
    string GetConnectionString(string name);

    int DeleteDataInTransaction<T>(StoredProcedure storedProcedure, T parameters);
    IReadOnlyList<T> LoadDataInTransaction<T, U>(StoredProcedure storedProcedure, U parameters);
    IReadOnlyList<T> LoadDataInTransaction<T>(StoredProcedure storedProcedure);
    int SaveDataInTransaction<T>(StoredProcedure storedProcedure, T parameters);
    int UpdateDataInTransaction<T>(StoredProcedure storedProcedure, T parameters);
}
