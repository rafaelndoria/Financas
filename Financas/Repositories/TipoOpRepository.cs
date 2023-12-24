using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class TipoOpRepository : ITipoOpRepository
    {
        private readonly IDbConnectionProvider _connection;
        string sql = "";

        public TipoOpRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public List<TipoOp> Get()
        {
            sql = "";
            sql = "SELECT * FROM TipoOp";
            return _connection.Connection.Query<TipoOp>(sql).ToList();
        }
    }
}
