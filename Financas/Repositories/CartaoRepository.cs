using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;
using Financas.ViewModels;
using System.Data;

namespace Financas.Repositories
{
    public class CartaoRepository : ICartaoRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public CartaoRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Delete(int cartaoId)
        {
            try
            {
                SQL = "";
                SQL = "DELETE FROM Cartao WHERE CartaoId = @Id";

                if (_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL, new { Id = cartaoId });
                }
                else
                {
                    _connection.Connection.Execute(SQL, new { Id = cartaoId }, _connection.CurrentTransaction);
                }
                    
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<Cartao> Get()
        {
            SQL = "";
            SQL = "SELECT * FROM Cartao";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<Cartao>(SQL).ToList();
            }

            return _connection.Connection.Query<Cartao>(SQL, _connection.CurrentTransaction).ToList();
        }

        public Cartao GetById(int id)
        {
            SQL = "";
            SQL = "SELECT * FROM Cartao WHERE CartaoId = @Id";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirst<Cartao>(SQL, new { Id = id });
            }

            return _connection.Connection.QueryFirst<Cartao>(SQL, new { Id = id }, _connection.CurrentTransaction);
        }

        public int GetCartaoPrincipal(int usuarioId)
        {
            SQL = "";
            SQL = "SELECT C.CartaoId FROM Cartao C JOIN Conta CT ON C.ContaId = CT.ContaId WHERE C.Principal = 1 AND CT.UsuarioId = @Id";

            if (_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<int>(SQL, new { Id = usuarioId });
            }
            
            return _connection.Connection.QueryFirstOrDefault<int>(SQL, new { Id = usuarioId }, _connection.CurrentTransaction);
        }

        public List<Cartao> GetCartaoUsuarioLogado(int usuarioId)
        {
            SQL = "";
            SQL = "SELECT Cc.* FROM Cartao Cc JOIN Conta C ON Cc.ContaId = C.ContaId WHERE C.UsuarioId = @Id";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<Cartao>(SQL, new { Id = usuarioId }).ToList();
            }

            return _connection.Connection.Query<Cartao>(SQL, new { Id = usuarioId }, _connection.CurrentTransaction).ToList();
        }

        public int GetDataVencimento(int cartaoId)
        {
            SQL = "";
            SQL = "SELECT DiaVencimento From Cartao WHERE CartaoId = @Id";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<int>(SQL, new { Id = cartaoId });
            }

            return _connection.Connection.QueryFirstOrDefault<int>(SQL, new { Id = cartaoId }, _connection.CurrentTransaction);
        }

        public bool Insert(Cartao cartao)
        {
            try
            {
                SQL = "";
                SQL = "INSERT INTO Cartao (Nome, LimiteCredito, Principal, LimiteCreditoAtual, DiaVencimento, ContaId) VALUES (@Nome, @LimiteCredito, @Principal, @LimiteCreditoAtual, @DiaVencimento, @ContaId)";

                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL, cartao);
                }
                else
                {
                    _connection.Connection.Execute(SQL, cartao, _connection.CurrentTransaction);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool PossuiCartaoPrincipal(int contaId)
        {
            SQL = "";
            SQL = "SELECT COUNT(*) FROM Cartao WHERE ContaId = @Id AND Principal = 1";

            int possuiCartao = 0;
            if(_connection.CurrentTransaction == null)
            {
                possuiCartao = _connection.Connection.Query<int>(SQL, new { Id = contaId }).Single();
            }
            else
            {
                possuiCartao = _connection.Connection.Query<int>(SQL, new { Id = contaId }, _connection.CurrentTransaction).Single();
            }

            if (possuiCartao == 0)
                return false;

            return true;
        }

        public bool RemoverCartaoPreferido()
        {
            try
            {
                SQL = "";
                SQL = "UPDATE Cartao SET Principal = 0 WHERE Principal = 1";

                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL);
                }
                else
                {
                    _connection.Connection.Execute(SQL, _connection.CurrentTransaction);
                }

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

                SQL = "";
                SQL = "UPDATE Cartao SET ";

                if (cartao.Nome != null && cartao.Nome.Length > 0)
                {
                    SQL += "Nome = @Nome,";
                    parametros.Add("Nome", cartao.Nome);
                    atualizar = true;
                }
                if (cartao.Principal > 0)
                {
                    if (cartao.Principal == 1)
                    {
                        RemoverCartaoPreferido();
                    }

                    SQL += "Principal = @Principal,";
                    parametros.Add("Princiapal", cartao.Principal);
                    atualizar = true;
                }
                if (cartao.LimiteCredito > 0)
                {
                    SQL += "LimiteCredito = @LimiteCredito,";
                    parametros.Add("LimiteCredito", cartao.LimiteCredito);
                    atualizar = true;
                }
                if (cartao.LimiteCreditoAtual > 0)
                {
                    SQL += "LimiteCreditoAtual = @LimiteCreditoAtual,";
                    parametros.Add("LimiteCreditoAtual", cartao.LimiteCreditoAtual);
                    atualizar = true;
                }
                if (cartao.DiaVencimento != 0)
                {
                    SQL += "DataVencimento = @DataVencimento,";
                    parametros.Add("DataVencimento", cartao.DiaVencimento);
                    atualizar = true;
                }

                if (!atualizar)
                    return false;

                if (SQL.EndsWith(","))
                {
                    SQL = SQL.Remove(SQL.Length - 1);
                }
                SQL += " WHERE CartaoId = @Id";

                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL, parametros);
                }
                else
                {
                    _connection.Connection.Execute(SQL, parametros, _connection.CurrentTransaction);
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
