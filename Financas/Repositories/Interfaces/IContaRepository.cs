using Financas.Models;
using Financas.ViewModels;

namespace Financas.Repositories.Interfaces
{
    public interface IContaRepository
    {
        List<Conta> Get(int? userId = null);
        Conta GetById(int id);
        bool Create(Conta conta);
        bool PossuiContaPrincipal(int id);
        bool Delete(int id);
        bool Update(int ContaId, ContaViewModel model, int usuarioId);
        bool VerificarExisteConta(int usuarioId, int contaId);
        bool RemoverContaPreferida();
        double GetSaldoConta(int contaId);
        int GetContaPrincipal(int usuarioId);
    }
}
