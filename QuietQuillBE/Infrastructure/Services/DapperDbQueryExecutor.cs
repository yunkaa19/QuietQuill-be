using System.Data;
using Application.Abstraction.Data;
using Dapper;

namespace Infrastructure.Services;

public class DapperDbQueryExecutor : IDbQueryExecutor
{
    private readonly IDbConnection _dbConnection;

    public DapperDbQueryExecutor(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return _dbConnection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
    }
    
    public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return _dbConnection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
    }
}