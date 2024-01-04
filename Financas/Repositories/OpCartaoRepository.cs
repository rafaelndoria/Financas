using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class OpCartaoRepository : IOpCartaoRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public OpCartaoRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Create(OpCartao opCartao)
        {
            try
            {
                SQL = "";
                SQL = @"INSERT INTO OpCartao (Descricao,Valor,Parcelado,QuantidadeParcelas,DataOp,ValorPorParcela,CartaoId,CategoriaOpId,TipoOpCartaoId)
	                VALUES (@Descricao,@Valor,@Parcelado,@QuantidadeParcelas,@DataOp,@ValorPorParcela,@CartaoId,@CategoriaOpId,@TipoOpCartaoId);";

                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Query<OpCartao>(SQL, opCartao);
                }

                _connection.Connection.Query<OpCartao>(SQL, opCartao, _connection.CurrentTransaction);

                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public List<OpCartao> Get()
        {
            SQL = "";
            SQL = "SELECT * FROM OpCartao";
            
            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<OpCartao>(SQL).ToList();
            }
            else
            {
                return _connection.Connection.Query<OpCartao>(SQL, _connection.CurrentTransaction).ToList();
            }
        }

        public OpCartao GetById(int opCartaoId)
        {
            SQL = "";
            SQL = "SELECT * FROM OpCartao WHERE OpCartaoId = @Id";

            if (_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<OpCartao>(SQL, new { Id = opCartaoId });
            }
            else
            {
                return _connection.Connection.QueryFirstOrDefault<OpCartao>(SQL, new { Id = opCartaoId }, _connection.CurrentTransaction);
            }
        }

        public List<OpCartao> GetOpCartoesUser(int usuarioId)
        {
            SQL = "";
            SQL = "SELECT* FROM OpCartao Op LEFT JOIN Cartao C ON Op.CartaoId = C.CartaoId LEFT JOIN Conta Cc ON C.ContaId = Cc.ContaId WHERE Cc.UsuarioId = @Id";

            if (_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<OpCartao>(SQL, new { Id = usuarioId }).ToList();
            }
            else
            {
                return _connection.Connection.Query<OpCartao>(SQL, new { Id = usuarioId }, _connection.CurrentTransaction).ToList();
            }
        }
    }
}
