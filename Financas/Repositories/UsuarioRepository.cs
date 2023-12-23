using Financas.Data;
using Financas.Models;
using Dapper;
using Financas.ViewModels;
using Financas.Services;
using Financas.Repositories.Interfaces;

namespace Financas.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnectionProvider _connection;
        string sql = "";

        public UsuarioRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Delete(int id)
        {
            try
            {
                sql = "";
                sql = "DELETE FROM Usuario WHERE UsuarioId = @Id";
                _connection.Connection.Execute(sql, new { Id = id });
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Update(int id, UsuarioViewModel usuario)
        {
            try
            {
                var atualizar = false;
                var parametros = new DynamicParameters();
                parametros.Add("Id", id);

                sql = "";
                sql = "UPDATE Usuario SET ";

                if(usuario.Nome.Length > 0)
                {
                    sql += "Nome = @Nome,";
                    parametros.Add("Nome", usuario.Nome);
                    atualizar = true;
                }
                if (usuario.Email.Length > 0)
                {
                    sql += "Email = @Email,";
                    parametros.Add("Email", usuario.Email);
                    atualizar = true;
                }
                if (usuario.Senha.Length > 0)
                {
                    var crypt = new CryptService();
                    var hash = crypt.CreateHashPassword(usuario.Senha);
                    sql += "Senha = @Senha,";
                    parametros.Add("Senha", hash);
                    atualizar = true;
                }

                if (!atualizar)
                    return false;

                if (sql.EndsWith(","))
                {
                    sql = sql.Remove(sql.Length - 1);
                }
                sql += " WHERE UsuarioId = @Id";

                _connection.Connection.Execute(sql, parametros);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Usuario> Get()
        {
            sql = "";
            sql = "SELECT * FROM Usuario";
            return _connection.Connection.Query<Usuario>(sql).ToList();
        }

        public Usuario GetByEmail(string email)
        {
            sql = "";
            sql = "SELECT * FROM Usuario U WHERE U.Email = @Email";
            return _connection.Connection.QueryFirstOrDefault<Usuario>(sql, new { Email = email });
        }

        public Usuario GetById(int id)
        {
            sql = "";
            sql = "SELECT * FROM Usuario U WHERE U.UsuarioId = @Id";
            return _connection.Connection.QueryFirstOrDefault<Usuario>(sql, new { Id = id });
        }

        public bool Insert(Usuario usuario)
        {
            try
            {
                sql = "";
                sql = @"INSERT INTO Usuario(Nome, Email, Senha, DataNascimento, DataCadastro)
                    VALUES(@Nome, @Email, @Senha, @DataNascimento, @DataCadastro);";
         
                _connection.Connection.Execute(sql, usuario);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetId(string email)
        {
            sql = "";
            sql = "SELECT UsuarioId FROM Usuario WHERE Email = @Email";
            return _connection.Connection.QueryFirstOrDefault<int>(sql, new { Email = email});
        }
    }
}
