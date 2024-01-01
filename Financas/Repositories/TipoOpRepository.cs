using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class TipoOpRepository : ITipoOpRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public TipoOpRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public List<TipoOp> Get()
        {
            SQL = "";
            SQL = "SELECT * FROM TipoOp";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<TipoOp>(SQL).ToList();
            }

            return _connection.Connection.Query<TipoOp>(SQL, _connection.CurrentTransaction).ToList();
        }
    }
}
