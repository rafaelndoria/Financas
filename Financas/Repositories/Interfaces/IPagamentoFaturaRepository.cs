using Financas.ViewModels;

namespace Financas.Repositories.Interfaces
{
    public interface IPagamentoFaturaRepository
    {
        bool Create(PagamentoFaturaViewModel fatura);
    }
}
