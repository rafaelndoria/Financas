using Dapper;
using Financas.Data;
using Financas.Models;
using Financas.Repositories.Interfaces;
using Financas.Services;
using Financas.ViewModels;

namespace Financas.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly IDbConnectionProvider _connection;
        string SQL = "";

        public ContaRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Create(Conta conta)
        {
            try
            {
                SQL = "";
                SQL = "INSERT INTO Conta (Nome, Principal, Balanco, UsuarioId) VALUES (@Nome,@Principal,@Balanco,@UsuarioId)";

                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Query<Conta>(SQL, conta);
                }
                else
                {
                    _connection.Connection.Query<Conta>(SQL, conta, _connection.CurrentTransaction);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                SQL = "";
                SQL = "DELETE FROM Conta WHERE ContaId = @Id";

                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL, new { Id = id });
                }
                else
                {
                    _connection.Connection.Execute(SQL, new { Id = id }, _connection.CurrentTransaction);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Conta> Get(int? userId = null)
        {
            if(userId == null)
            {
                SQL = "";
                SQL = "SELECT * FROM Conta";

                if(_connection.CurrentTransaction == null)
                {
                    return _connection.Connection.Query<Conta>(SQL).ToList();
                }

                return _connection.Connection.Query<Conta>(SQL, _connection.CurrentTransaction).ToList();
            }
            else
            {
                SQL = "";
                SQL = "SELECT * FROM Conta WHERE UsuarioId = @Id";

                if(_connection.CurrentTransaction == null)
                {
                    return _connection.Connection.Query<Conta>(SQL, new { Id = userId }).ToList();
                }

                return _connection.Connection.Query<Conta>(SQL, new { Id = userId }, _connection.CurrentTransaction).ToList();
            }
            
        }

        public Conta GetById(int id)
        {
            SQL = "";
            SQL = "SELECT * FROM Conta WHERE ContaId = @Id";

            if (_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<Conta>(SQL, new { Id = id });
            }

            return _connection.Connection.QueryFirstOrDefault<Conta>(SQL, new { Id = id }, _connection.CurrentTransaction);
        }

        public int GetContaPrincipal(int usuarioId)
        {
            SQL = "";
            SQL = "SELECT C.ContaId FROM Conta C WHERE C.Principal = 1 AND C.UsuarioId = @Id";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<int>(SQL, new { Id = usuarioId });
            }
            else
            {
                return _connection.Connection.QueryFirstOrDefault<int>(SQL, new { Id = usuarioId }, _connection.CurrentTransaction);
            }
        }

        public double GetSaldoConta(int contaId)
        {
            SQL = "";
            SQL = "SELECT Balanco FROM Conta WHERE ContaId = @Id";

            if (_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<double>(SQL, new { Id = contaId });
            }

            return _connection.Connection.QueryFirstOrDefault<double>(SQL, new { Id = contaId }, _connection.CurrentTransaction);
        }

        public bool PossuiContaPrincipal(int id)
        {
            SQL = "";
            SQL = "SELECT COUNT(*) FROM CONTA C WHERE C.Principal = 1 AND C.UsuarioId = @Id";

            int possuiConta = 0;
            if(_connection.CurrentTransaction == null)
            {
                possuiConta = _connection.Connection.Query<int>(SQL, new { Id = id }).Single();
            }
            else
            {
                possuiConta = _connection.Connection.Query<int>(SQL, new { Id = id }, _connection.CurrentTransaction).Single();
            }
            if(possuiConta == 0)
                return false;

            return true;
        }

        public bool RemoverContaPreferida()
        {
            SQL = "";
            SQL = "UPDATE Conta SET Principal = 0 WHERE Principal = 1";

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

        public bool Update(int ContaId, ContaViewModel conta, int usuarioId)
        {
            try
            {
                var atualizar = false;
                var parametros = new DynamicParameters();
                parametros.Add("Id", ContaId);

                SQL = "";
                SQL = "UPDATE Conta SET ";

                if (conta.Nome != null && conta.Nome.Length == 0)
                {
                    SQL += "Nome = @Nome,";
                    parametros.Add("Nome", conta.Nome);
                    atualizar = true;
                }
                if (conta.Principal > 0)
                {
                    if (conta.Principal == 1)
                    {
                        RemoverContaPreferida();
                    }

                    SQL += "Principal = @Principal,";
                    parametros.Add("Princiapal", conta.Principal);
                    atualizar = true;
                }
                if (conta.Balanco > 0)
                {
                    SQL += "Balanco = @Balanco,";
                    parametros.Add("Balanco", conta.Balanco);
                    atualizar = true;
                }

                if (!atualizar)
                    return false;

                if (SQL.EndsWith(","))
                {
                    SQL = SQL.Remove(SQL.Length - 1);
                }
                SQL += " WHERE ContaId = @Id";

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

        public bool VerificarExisteConta(int usuarioId, int contaId)
        {
            SQL = "";
            SQL = "SELECT COUNT(*) FROM Conta WHERE ContaId = @ContaId AND UsuarioId = @UsuarioId";

            int possuiConta = 0;
            if(_connection.CurrentTransaction == null)
            {
                possuiConta = _connection.Connection.Query<int>(SQL, new { ContaId = contaId, UsuarioId = usuarioId }).Single();
            }
            else
            {
                possuiConta = _connection.Connection.Query<int>(SQL, new { ContaId = contaId, UsuarioId = usuarioId }, _connection.CurrentTransaction).Single();
            }
            if (possuiConta == 0)
                return false;

            return true;
        }
    }
}
