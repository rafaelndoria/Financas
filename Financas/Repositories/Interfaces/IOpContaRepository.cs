using Financas.Models;

namespace Financas.Repositories.Interfaces
{
    public interface IOpContaRepository
    {
        List<OpConta> Get();
        OpConta GetById(int id);
        List<OpConta> GetOpContaUsuarioLogado(int usuarioId);
        bool Create(OpConta opConta);
    }
}
