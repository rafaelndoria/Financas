using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class FaturaRepository : IFaturaRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public FaturaRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public List<Fatura> Get()
        {
            SQL = "";
            SQL = "SELECT * FROM Fatura";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<Fatura>(SQL).ToList();
            }
            else
            {
                return _connection.Connection.Query<Fatura>(SQL, _connection.CurrentTransaction).ToList();
            }
        }

        public Fatura GetById(int id)
        {
            SQL = "";
            SQL = "SELECT * FROM Fatura Where FaturaId = @Id";

            if (_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<Fatura>(SQL, new { Id = id });
            }
            else
            {
                return _connection.Connection.QueryFirstOrDefault<Fatura>(SQL, new { Id = id }, _connection.CurrentTransaction);
            }
        }

        public Fatura GetByMesAno(int mes, int ano, int usuarioId)
        {
            SQL = "";
            SQL = "SELECT F.* FROM Fatura F JOIN Cartao C ON F.CartaoId = C.CartaoId JOIN Conta Cc ON Cc.ContaId = C.ContaId WHERE Cc.UsuarioId = @Id AND F.Mes = @Mes AND F.Ano = @Ano";

            if (_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<Fatura>(SQL, new { Id = usuarioId, Mes = mes, Ano = ano });
            }
            else
            {
                return _connection.Connection.QueryFirstOrDefault<Fatura>(SQL, new { Id = usuarioId, Mes = mes, Ano = ano }, _connection.CurrentTransaction);
            }
        }

        public List<Fatura> GetFaturaUsuarioLogado(int usuarioId)
        {
            SQL = "";
            SQL = "SELECT F.* FROM Fatura F JOIN Cartao C ON F.CartaoId = C.CartaoId JOIN Conta Cc ON Cc.ContaId = C.ContaId WHERE Cc.UsuarioId = @Id ORDER BY F.Mes, F.Ano DESC";

            if (_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<Fatura>(SQL, new { Id = usuarioId }).ToList();
            }
            else
            {
                return _connection.Connection.Query<Fatura>(SQL, new { Id = usuarioId }, _connection.CurrentTransaction).ToList();
            }
        }

        public Fatura GetOrInsert(int cartaoId, int mes, int ano)
        {
            SQL = "";
            SQL = "SELECT * FROM Fatura WHERE CartaoId = @Id AND Mes = @Mes AND Ano = @Ano";

            Fatura fatura;
            if(_connection.CurrentTransaction == null)
            {
                fatura = _connection.Connection.QueryFirstOrDefault<Fatura>(SQL, new { Id = cartaoId, Mes = mes, Ano = ano });
            }
            else
            {
                fatura = _connection.Connection.QueryFirstOrDefault<Fatura>(SQL, new { Id = cartaoId, Mes = mes, Ano = ano }, _connection.CurrentTransaction);
            }
   
            if (fatura == null)
            {
                SQL = "";
                SQL = "INSERT INTO Fatura (Mes, Ano, CartaoId, Valor) VALUES (@Mes, @Ano, @CartaoId, 0); SELECT SCOPE_IDENTITY() AS FaturaId;";

                if(_connection.CurrentTransaction == null)
                {
                    fatura = _connection.Connection.QueryFirstOrDefault<Fatura>(SQL, new { CartaoId = cartaoId, Mes = mes, Ano = ano });
                }
                else
                {
                    fatura = _connection.Connection.QueryFirstOrDefault<Fatura>(SQL, new { CartaoId = cartaoId, Mes = mes, Ano = ano }, _connection.CurrentTransaction);
                }
            }

            return fatura;
        }

        public bool Update(double valor, int faturaId)
        {
            SQL = "";
            SQL = "UPDATE Fatura SET Valor = @Valor WHERE FaturaId = @Id";

            try
            {
                if(_connection.CurrentTransaction == null) 
                {
                    _connection.Connection.Execute(SQL, new { Valor = valor, Id = faturaId });
                }
                else
                {
                    _connection.Connection.Execute(SQL, new { Valor = valor, Id = faturaId }, _connection.CurrentTransaction);
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
