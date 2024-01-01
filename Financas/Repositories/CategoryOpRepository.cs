using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class CategoriaOpRepository : ICategoriaOpRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public CategoriaOpRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public List<CategoriaOp> Get()
        {
            SQL = "";
            SQL = "SELECT * FROM CategoriaOp";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<CategoriaOp>(SQL).ToList();
            }

            return _connection.Connection.Query<CategoriaOp>(SQL, _connection.CurrentTransaction).ToList();
        }
    }
}
