using System.Data.SqlClient;
using System.Data;
using Financas.Data;

public class DbConnectionProvider : IDbConnectionProvider
{
    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;

    public DbConnectionProvider(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
    }

    public IDbConnection Connection => _connection;

    public IDbTransaction BeginTransaction()
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Já existe uma transação em andamento.");
        }

        _transaction = _connection.BeginTransaction();
        return _transaction;
    }

    public void Commit()
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Nenhuma transação em andamento para confirmar.");
        }

        _transaction.Commit();
        _transaction = null;
    }

    public void Dispose()
    {
        if (_connection.State != ConnectionState.Closed)
        {
            _connection.Close();
        }
    }

    public void Rollback()
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Nenhuma transação em andamento para reverter.");
        }

        _transaction.Rollback();
        _transaction = null;
    }
}
