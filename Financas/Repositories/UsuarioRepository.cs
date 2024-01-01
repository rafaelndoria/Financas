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
        string SQL = "";

        public UsuarioRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }

        public bool Delete(int id)
        {
            try
            {
                SQL = "";
                SQL = "DELETE FROM Usuario WHERE UsuarioId = @Id";

                if (_connection.CurrentTransaction == null)
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

        public bool Update(int id, UsuarioViewModel usuario)
        {
            try
            {
                var atualizar = false;
                var parametros = new DynamicParameters();
                parametros.Add("Id", id);

                SQL = "";
                SQL = "UPDATE Usuario SET ";

                if(usuario.Nome.Length > 0)
                {
                    SQL += "Nome = @Nome,";
                    parametros.Add("Nome", usuario.Nome);
                    atualizar = true;
                }
                if (usuario.Email.Length > 0)
                {
                    SQL += "Email = @Email,";
                    parametros.Add("Email", usuario.Email);
                    atualizar = true;
                }
                if (usuario.Senha.Length > 0)
                {
                    var crypt = new CryptService();
                    var hash = crypt.CreateHashPassword(usuario.Senha);
                    SQL += "Senha = @Senha,";
                    parametros.Add("Senha", hash);
                    atualizar = true;
                }

                if (!atualizar)
                    return false;

                if (SQL.EndsWith(","))
                {
                    SQL = SQL.Remove(SQL.Length - 1);
                }
                SQL += " WHERE UsuarioId = @Id";

                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL, parametros);
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

        public List<Usuario> Get()
        {
            SQL = "";
            SQL = "SELECT * FROM Usuario";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.Query<Usuario>(SQL).ToList();
            }

            return _connection.Connection.Query<Usuario>(SQL, _connection.CurrentTransaction).ToList();
        }

        public Usuario GetByEmail(string email)
        {
            SQL = "";
            SQL = "SELECT * FROM Usuario U WHERE U.Email = @Email";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<Usuario>(SQL, new { Email = email });
            }

            return _connection.Connection.QueryFirstOrDefault<Usuario>(SQL, new { Email = email }, _connection.CurrentTransaction);
        }

        public Usuario GetById(int id)
        {
            Usuario usuario = null;

            SQL = "";
            SQL = "SELECT U.*, C.* FROM Usuario U LEFT JOIN Conta C ON U.UsuarioId = C.UsuarioId WHERE U.UsuarioId = @Id";

            if(_connection.CurrentTransaction == null)
            {
                _connection.Connection.Query<Usuario, Conta, Usuario>(
                SQL,
                (usuarioConsulta, contaConsulta) =>
                {
                    if (usuario == null)
                    {
                        usuario = usuarioConsulta;
                        usuario.Contas = new List<Conta>();
                    }
                    usuario.Contas.Add(contaConsulta);
                    return usuario;
                },
                new { Id = id },
                splitOn: "UsuarioId,ContaId");
            }
            else
            {
                _connection.Connection.Query<Usuario, Conta, Usuario>(
               SQL,
               (usuarioConsulta, contaConsulta) =>
               {
                   if (usuario == null)
                   {
                       usuario = usuarioConsulta;
                       usuario.Contas = new List<Conta>();
                   }
                   usuario.Contas.Add(contaConsulta);
                   return usuario;
               },
               new { Id = id },
               _connection.CurrentTransaction,
               splitOn: "UsuarioId,ContaId");
            }
           

            return usuario;
        }

        public bool Insert(Usuario usuario)
        {
            try
            {
                SQL = "";
                SQL = @"INSERT INTO Usuario(Nome, Email, Senha, DataNascimento, DataCadastro)
                    VALUES(@Nome, @Email, @Senha, @DataNascimento, @DataCadastro);";
         
                if(_connection.CurrentTransaction == null)
                {
                    _connection.Connection.Execute(SQL, usuario);
                }
                else
                {
                    _connection.Connection.Execute(SQL, usuario, _connection.CurrentTransaction);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetId(string email)
        {
            SQL = "";
            SQL = "SELECT UsuarioId FROM Usuario WHERE Email = @Email";

            if(_connection.CurrentTransaction == null)
            {
                return _connection.Connection.QueryFirstOrDefault<int>(SQL, new { Email = email });
            }

            return _connection.Connection.QueryFirstOrDefault<int>(SQL, new { Email = email }, _connection.CurrentTransaction);
        }
    }
}
