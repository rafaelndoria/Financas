using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class CategoriaOpRepository : ICategoriaOpRepository
    {
        private readonly IDbConnectionProvider _connection;
        string sql = "";

        public CategoriaOpRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public List<CategoriaOp> Get()
        {
            sql = "";
            sql = "SELECT * FROM CategoriaOp";
            return _connection.Connection.Query<CategoriaOp>(sql).ToList();
        }
    }
}
