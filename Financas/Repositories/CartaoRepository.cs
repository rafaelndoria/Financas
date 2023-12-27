using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;
using Financas.ViewModels;

namespace Financas.Repositories
{
    public class CartaoRepository : ICartaoRepository
    {
        private readonly IDbConnectionProvider _connection;
        string sql = "";

        public CartaoRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Delete(int cartaoId)
        {
            try
            {
                sql = "";
                sql = "DELETE FROM Cartao WHERE CartaoId = @Id";
                _connection.Connection.Execute(sql, new { Id = cartaoId });
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<Cartao> Get()
        {
            sql = "";
            sql = "SELECT * FROM Cartao";
            return _connection.Connection.Query<Cartao>(sql).ToList();
        }

        public Cartao GetById(int id)
        {
            sql = "";
            sql = "SELECT * FROM Cartao WHERE CartaoId = @Id";
            return _connection.Connection.QueryFirst<Cartao>(sql, new { Id = id });
        }

        public int GetCartaoPrincipal(int usuarioId)
        {
            sql = "";
            sql = "SELECT C.CartaoId FROM Cartao C JOIN Conta CT ON C.ContaId = CT.ContaId WHERE C.Principal = 1 AND CT.UsuarioId = @Id";
            return _connection.Connection.QueryFirstOrDefault<int>(sql, new { Id = usuarioId});
        }

        public List<Cartao> GetCartaoUsuarioLogado(int usuarioId)
        {
            sql = "";
            sql = "SELECT Cc.* FROM Cartao Cc JOIN Conta C ON Cc.ContaId = C.ContaId WHERE C.UsuarioId = @Id";
            return _connection.Connection.Query<Cartao>(sql, new { Id = usuarioId }).ToList();
        }

        public bool Insert(Cartao cartao)
        {
            try
            {
                sql = "";
                sql = "INSERT INTO Cartao (Nome, LimiteCredito, Principal, LimiteCreditoAtual, ContaId) VALUES (@Nome, @LimiteCredito, @Principal, @LimiteCreditoAtual, @ContaId)";

                _connection.Connection.Execute(sql, cartao);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool PossuiCartaoPrincipal(int contaId)
        {
            sql = "";
            sql = "SELECT COUNT(*) FROM Cartao WHERE ContaId = @Id AND Principal = 1";
            var possuiCartao = _connection.Connection.Query<int>(sql, new { Id = contaId }).Single();

            if (possuiCartao == 0)
                return false;

            return true;
        }

        public bool RemoverCartaoPreferido()
        {
            try
            {
                sql = "";
                sql = "UPDATE Cartao SET Principal = 0 WHERE Principal = 1";
                _connection.Connection.Execute(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(int cartaoId, CartaoViewModel cartao)
        {
            try
            {
                var atualizar = false;
                var parametros = new DynamicParameters();
                parametros.Add("Id", cartaoId);

                sql = "";
                sql = "UPDATE Cartao SET ";

                if (cartao.Nome.Length > 0)
                {
                    sql += "Nome = @Nome,";
                    parametros.Add("Nome", cartao.Nome);
                    atualizar = true;
                }
                if (cartao.Principal > 0)
                {
                    if (cartao.Principal == 1)
                    {
                        RemoverCartaoPreferido();
                    }

                    sql += "Principal = @Principal,";
                    parametros.Add("Princiapal", cartao.Principal);
                    atualizar = true;
                }
                if (cartao.LimiteCredito > 0)
                {
                    sql += "LimiteCredito = @LimiteCredito,";
                    parametros.Add("LimiteCredito", cartao.LimiteCredito);
                    atualizar = true;
                }
                if (cartao.DataVencimento.Length > 0)
                {
                    sql += "DataVencimento = @DataVencimento,";
                    parametros.Add("DataVencimento", DateTime.Parse(cartao.DataVencimento));
                    atualizar = true;
                }

                if (!atualizar)
                    return false;

                if (sql.EndsWith(","))
                {
                    sql = sql.Remove(sql.Length - 1);
                }
                sql += " WHERE CartaoId = @Id";

                _connection.Connection.Execute(sql, parametros);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
