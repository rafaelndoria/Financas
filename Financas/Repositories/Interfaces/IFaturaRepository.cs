using Financas.Models;

namespace Financas.Repositories.Interfaces
{
    public interface IFaturaRepository
    {
        Fatura GetOrInsert(int cartaoId, int mes, int ano);
        bool Update(double valor, int faturaId);
        List<Fatura> Get();
        List<Fatura> GetFaturaUsuarioLogado(int usuarioId);
        Fatura GetById(int id);
        Fatura GetByMesAno(int mes, int ano, int usuarioId);
    }
}
