using Financas.Models;
using Financas.ViewModels;

namespace Financas.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        bool Insert(Usuario usuario);
        List<Usuario> Get();
        Usuario GetById(int id);
        Usuario GetByEmail(string email);
        bool Delete(int id);
        bool Update(int id, UsuarioViewModel usuario);
        int GetId(string email);
    }
}
