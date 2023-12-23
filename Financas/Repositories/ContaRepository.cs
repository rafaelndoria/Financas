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
        string sql = "";

        public ContaRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Create(Conta conta)
        {
            try
            {
                sql = "";
                sql = "INSERT INTO Conta (Nome, Principal, Balanco, UsuarioId) VALUES (@Nome,@Principal,@Balanco,@UsuarioId)";
                _connection.Connection.Query<Conta>(sql, conta);

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
                sql = "";
                sql = "DELETE FROM Conta WHERE ContaId = @Id";
                _connection.Connection.Execute(sql, new { Id = id });

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
                sql = "";
                sql = "SELECT * FROM Conta";
                return _connection.Connection.Query<Conta>(sql).ToList();
            }
            else
            {
                sql = "";
                sql = "SELECT * FROM Conta WHERE UsuarioId = @Id";
                return _connection.Connection.Query<Conta>(sql, new { Id = userId }).ToList();
            }
            
        }

        public Conta GetById(int id)
        {
            sql = "";
            sql = "SELECT * FROM Conta WHERE ContaId = @Id";
            return _connection.Connection.QueryFirstOrDefault<Conta>(sql, new { Id = id });
        }

        public bool PossuiContaPrincipal(int id)
        {
            sql = "";
            sql = "SELECT COUNT(*) FROM CONTA C WHERE C.Principal = 1 AND C.UsuarioId = @Id";

            var possuiConta = _connection.Connection.Query<int>(sql, new { Id = id }).Single();
            if(possuiConta == 0)
                return false;

            return true;
        }

        public bool Update(int ContaId, ContaViewModel conta, int usuarioId)
        {
            try
            {
                var atualizar = false;
                var parametros = new DynamicParameters();
                parametros.Add("Id", ContaId);

                sql = "";
                sql = "UPDATE Conta SET ";

                if (conta.Nome.Length > 0)
                {
                    sql += "Nome = @Nome,";
                    parametros.Add("Nome", conta.Nome);
                    atualizar = true;
                }
                if (conta.Principal > 0)
                {
                    if (conta.Principal == 1 && PossuiContaPrincipal(usuarioId))
                        return false;

                    sql += "Principal = @Principal,";
                    parametros.Add("Princiapal", conta.Principal);
                    atualizar = true;
                }
            
                if (!atualizar)
                    return false;

                if (sql.EndsWith(","))
                {
                    sql = sql.Remove(sql.Length - 1);
                }
                sql += " WHERE ContaId = @Id";

                _connection.Connection.Execute(sql, parametros);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool VerificarExisteConta(int usuarioId, int contaId)
        {
            sql = "";
            sql = "SELECT COUNT(*) FROM Conta WHERE ContaId = @ContaId AND UsuarioId = @UsuarioId";
            var possuiConta = _connection.Connection.Query<int>(sql, new { ContaId = contaId, UsuarioId = usuarioId }).Single();

            if (possuiConta == 0)
                return false;

            return true;
        }
    }
}
