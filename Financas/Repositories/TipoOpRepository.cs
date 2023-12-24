using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class TipoOpRepository : ITipoOpCartaoRepository
    {
        private readonly IDbConnectionProvider _connection;
        string sql = "";

        public TipoOpRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public List<TipoOpCartao> Get()
        {
            sql = "";
            sql = "SELECT * FROM TipoOpCartao";
            return _connection.Connection.Query<TipoOpCartao>(sql).ToList();
        }
    }
}
