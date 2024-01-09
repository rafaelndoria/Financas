using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class OpContaRepository : IOpContaRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public OpContaRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Create(OpConta opConta)
        {
            try
            {
                SQL = "";
                SQL = "INSERT INTO OpConta (Descricao, Valor, DataOp, ContaId, CategoriaOpId, TipoOpId) VALUES (@Descricao, @Valor, @DataOp, @ContaId, @CategoriaOpId, @TipoOpId)";

                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL, opConta);
                }
                else
                {
                    _connection.Connection.Execute(SQL, opConta, _connection.CurrentTransaction);
                }

                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public List<OpConta> Get()
        {
            SQL = "";
            SQL = "SELECT * FROM OpConta";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<OpConta>(SQL).ToList();
            }
            else
            {
                return _connection.Connection.Query<OpConta>(SQL, _connection.CurrentTransaction).ToList();
            }
        }

        public OpConta GetById(int id)
        {
            SQL = "";
            SQL = "SELECT * FROM OpConta WHERE OpContaId = @Id";

            if (_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<OpConta>(SQL, new { Id = id});
            }
            else
            {
                return _connection.Connection.QueryFirstOrDefault<OpConta>(SQL, new { Id = id }, _connection.CurrentTransaction);
            }
        }

        public List<OpConta> GetOpContaUsuarioLogado(int usuarioId)
        {
            SQL = "";
            SQL = "SELECT * FROM OpConta Oc JOIN Conta C ON Oc.ContaId = C.ContaId WHERE C.UsuarioId = @Id";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<OpConta>(SQL, new { Id = usuarioId }).ToList();
            }
            else
            {
                return _connection.Connection.Query<OpConta>(SQL, new { Id = usuarioId }, _connection.CurrentTransaction).ToList();
            }
        }
    }
}
