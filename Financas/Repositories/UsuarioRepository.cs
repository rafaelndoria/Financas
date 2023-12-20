using Dapper;
using Financas.Data;

namespace Financas.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnectionProvider _connection;

        public UsuarioRepository(IDbConnectionProvider connection)
        {
            _connection = connection;
        }
    }
}
