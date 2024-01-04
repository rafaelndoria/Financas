using Financas.Models;

namespace Financas.Repositories.Interfaces
{
    public interface IOpCartaoRepository
    {
        bool Create(OpCartao opCartao);
        List<OpCartao> Get();
        OpCartao GetById(int opCartaoId);
        List<OpCartao> GetOpCartoesUser(int usuarioId);
    }
}
