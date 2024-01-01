using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class TipoOpCartaoRepository : ITipoOpCartaoRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public TipoOpCartaoRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public List<TipoOpCartao> Get()
        {
            SQL = "";
            SQL = "SELECT * FROM TipoOpCartao";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<TipoOpCartao>(SQL).ToList();
            }

            return _connection.Connection.Query<TipoOpCartao>(SQL, _connection.CurrentTransaction).ToList();

        }
    }
}
