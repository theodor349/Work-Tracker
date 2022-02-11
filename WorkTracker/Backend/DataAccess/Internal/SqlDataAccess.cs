using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Internal;

public abstract class SqlDataAccess : ISqlDataAccess, IDisposable
{
    private readonly IConfiguration _config;
    private readonly ILogger<SqlDataAccess> _logger;

    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private string _connectionString;
    private int _openConnections = 0;
    private bool _isClosed => _openConnections == 0;

    public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger, string dbName)
    {
        _config = config;
        _logger = logger;
        _connectionString = GetConnectionString(dbName);
    }

    public int SaveDataInTransaction<T>(StoredProcedure storedProcedure, T parameters)
    {
        return _connection.Execute(
            storedProcedure.ToString(), parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction);
    }

    public IReadOnlyList<T> LoadDataInTransaction<T, U>(StoredProcedure storedProcedure, U parameters)
    {
        var rows = _connection.Query<T>(
            storedProcedure.ToString(), parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
        return rows;
    }
    public IReadOnlyList<T> LoadDataInTransaction<T>(StoredProcedure storedProcedure)
    {
        var rows = _connection.Query<T>(
            storedProcedure.ToString(),
            commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
        return rows;
    }

    public int UpdateDataInTransaction<T>(StoredProcedure storedProcedure, T parameters)
    {
        return SaveDataInTransaction(storedProcedure, parameters);
    }

    public int DeleteDataInTransaction<T>(StoredProcedure storedProcedure, T parameters)
    {
        return _connection.Execute(
            storedProcedure.ToString(), parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction);
    }

    public void StartTransaction()
    {
        if (_isClosed)
        {
            _logger.LogTrace("Transaction: Start");
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }
        _openConnections++;
    }

    public void CommitTransaction()
    {
        _openConnections--;
        if (_isClosed)
        {
            _logger.LogTrace("Transaction: Commit");
            _transaction?.Commit();
            _connection?.Close();
        }
    }

    public void RollbackTransaction()
    {
        _logger.LogTrace("Transaction: Rollback");
        _transaction?.Rollback();
        _connection?.Close();
        _openConnections = 0;
    }

    public string GetConnectionString(string name)
    {
        return _config.GetConnectionString(name);
    }

    public void Dispose()
    {
        if (!_isClosed)
        {
            try
            {
                CommitTransaction();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Commit transaction failed in the dispose method.");
            }
        }
        _connection = null;
        _transaction = null;
    }
}
